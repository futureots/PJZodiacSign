using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


namespace PlayerInput
{
    public class SkillModeInput : IInputState, IInput
    {
        readonly InputManager _inputManager;

        private List<GameObject> selecters;

        SkillComponent skillComp;

        public Action onCanceled;
        public SkillModeInput(InputManager input, SkillComponent skill)
        {
            _inputManager = input;
            skillComp = skill;

            selecters = new();
        }

        public void RemoveMode()
        {
        }

        public void SetMode()
        {
            _ = InputSkill(skillComp);
        }

        async UniTaskVoid InputSkill(SkillComponent skill)
        {
            var task = await skill.skillLogic.InputSkill(this);
            
            // 스킬 입력이 종료되면 스킬이 종료될 때까지 대기 한 후 기본 입력 모드로 변경
            var flag = true;
            Action<int> wait = i =>
            {
                EditorLogger.Print(flag +" :: "+ i);
                if (i == 0) flag = true;
                else flag = false;
            };
            StageManager.Instance.isSequencing += wait;
            // 정상 완료 시 커맨드 생성 및 스킬 입력 모드 종료
            if (task)
            {
                // 커맨드 생성
                var command = _inputManager.agent.CreateSkillCommand(skill);
                command.indicate.AddRange(selecters);
                EditorLogger.Print("SkillCreated");
            }
            else
            {
                foreach (GameObject go in selecters)
                {
                    Object.Destroy(go);
                }
                selecters.Clear();
            }
            await new WaitUntil(() => flag);
            StageManager.Instance.isSequencing -= wait;
            
            _inputManager.SetInputMode();
        }

        public async UniTask<List<Entity>> InputEntity(List<Entity> list, int count = -1)
        {
            if (list.Count <= 0) return null;
            if (list.Count <= count) return new List<Entity>(list);
            
            var selectedEntities = new List<Entity>();
            bool isCanceled = false;

            UnityAction<GameObject> click = (obj) =>
            {
                if (obj && obj.TryGetComponent<Entity>(out var entity))
                {
                    if (list.Contains(entity) && !selectedEntities.Contains(entity))
                    {
                        selectedEntities.Add(entity);
                        entity.RemoveHighlight();
                        selecters.Add(Object.Instantiate(_inputManager.skillSelecter,entity.CurTile.transform.position + Vector3.up*0.1f, Utils.QI));
                    }
                }
            };
            Action cancel = () => isCanceled = true;
            
            list.ForEach(e => e.ApplyHighlight());
            _inputManager.OnObjectClicked.AddListener(click);
            onCanceled += cancel;
            
            await UniTask.WaitUntil(() => isCanceled || selectedEntities.Count >= count);
            
            list.ForEach(e => e.RemoveHighlight());
            _inputManager.OnObjectClicked.RemoveListener(click);
            onCanceled -= cancel;
            
            if (isCanceled) return null;
            
            return selectedEntities;
        }

        public async UniTask<List<Tile>> InputTile(List<Tile> list, int maxCount = -1)
        {
            if (list.Count <= 0) return null;
            if (list.Count <= maxCount) return new List<Tile>(list);
            
            var selectedTiles = new List<Tile>();
            bool isCanceled = false;

            UnityAction<GameObject> click = (obj) =>
            {
                if (obj && obj.TryGetComponent<Tile>(out var tile))
                {
                    if (list.Contains(tile) && !selectedTiles.Contains(tile))
                    {
                        selectedTiles.Add(tile);
                        tile.RemoveHighlight(Tile.HighLightType.Move);
                        selecters.Add(Object.Instantiate(_inputManager.skillSelecter,tile.transform.position + Vector3.up*0.1f, Utils.QI));
                    }
                }
            };
            Action cancel = () => isCanceled = true;
            
            list.ForEach(e => e.ApplyHighlight(Tile.HighLightType.Move));
            _inputManager.OnObjectClicked.AddListener(click);
            onCanceled += cancel;
            
            await UniTask.WaitUntil(() => isCanceled || selectedTiles.Count >= maxCount);
            if (isCanceled) return null;
            
            list.ForEach(e => e.RemoveHighlight(Tile.HighLightType.Move));
            _inputManager.OnObjectClicked.RemoveListener(click);
            onCanceled -= cancel;
            
            return selectedTiles;
        }
    }
}
