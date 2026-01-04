using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace PlayerInput
{
    public class SkillModeInput : IModeInput, IInput
    {
        

        InputManager _inputManager;

        List<GameObject> selecters;

        SkillComponent skillComp;
        public SkillModeInput(InputManager input, SkillComponent skill)
        {
            _inputManager = input;
            skillComp = skill;

            selecters = new();
        }

        public void RemoveMode()
        {
            _inputManager.UI.cancelButton.gameObject.SetActive(false);
        }

        public void SetMode()
        {
            _inputManager.UI.cancelButton.gameObject.SetActive(true);

            _inputManager.StartCoroutine(InputSkill(skillComp));
        }
        
        // 스킬 입력 및 커맨드 생성 코루틴
        public IEnumerator InputSkill(SkillComponent skill)
        {
            bool isCompleted = false;
            Action<bool> action = (x) => { isCompleted = x; };
            yield return skill.StartCoroutine(skill.skillLogic.InputSkill(this,action));
            // TODO : 정상 완료 시 커맨드 생성 및 스킬 입력 모드 종료
            if (isCompleted)
            {
                // NOTE : 커맨드 생성
                _inputManager.agent.CreateSkillCommand(skill);
            }
            _inputManager.SetInputMode();
            yield break;
        }

        public IEnumerator InputEntity(List<Entity> list, Action<Entity> input, Action<bool> callback, int maxCount = -1)
        {
            bool isCanceled = false;
            if (list.Count <= maxCount)
            {
                foreach (Entity entity in list)
                {
                    input?.Invoke(entity);
                }
                callback?.Invoke(true);
                yield break;
            }
            int count = 0;
            UnityAction<GameObject> click = (x) =>
            {
                if (x.TryGetComponent<Entity>(out var entity))
                {
                    if (list.Contains(entity))
                    {
                        input?.Invoke(entity);
                        count++;
                    }
                }
            };
            // TODO : list 기물 시각화

            //TODO : 취소버튼 설정하기
            _inputManager.OnObjectClicked.AddListener(click);
            yield return new WaitUntil(()=>  { return isCanceled || count>=maxCount; });
            _inputManager.OnObjectClicked.RemoveListener(click);

            // TODO : 시각화 제거
            if (isCanceled)
            {
                callback?.Invoke(false);
                yield break;
            }
            callback?.Invoke(true);
        }

        public IEnumerator InputTile(List<Tile> list, Action<Tile> input, Action<bool> callback, int maxCount = -1)
        {
            bool isCanceled = false;
            if (list.Count <= maxCount)
            {
                foreach (Tile tile in list)
                {
                    input?.Invoke(tile);
                }
                callback?.Invoke(true);
                yield break;
            }
            int count = 0;
            UnityAction<GameObject> click = (x) =>
            {
                if (x.TryGetComponent<Tile>(out var tile))
                {
                    if (list.Contains(tile))
                    {
                        input?.Invoke(tile);
                        count++;
                    }
                }
            };
            // TODO : list 기물 시각화

            //TODO : 취소버튼 설정하기
            _inputManager.OnObjectClicked.AddListener(click);
            yield return new WaitUntil(() => { return isCanceled || count >= maxCount; });
            _inputManager.OnObjectClicked.RemoveListener(click);

            // TODO : 시각화 제거
            if (isCanceled)
            {
                callback?.Invoke(false);
                yield break;
            }
            callback?.Invoke(true);
        }

    }
}
