using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerInput
{
    public class SkillModeInput : IModeInput
    {
        IPhaseManageService phaseService;

        IActive skill;
        
        Action<InputAction.CallbackContext> bindAction;

        InputManager _inputManager;

        List<GameObject> selecters;

        Queue<FieldInfo> skillFields;
        FieldInfo currentField;

        SkillComponent skillComp;

        public SkillModeInput(InputManager input, IActive skill, IPhaseManageService phaseService)
        {
            this.phaseService = phaseService;
            this.skill = skill;
            _inputManager = input;
            // 스킬에 필요한 입력이 필요한 필드 큐 생성
            selecters = new();
            skillFields = new();
            var type = skill.GetType();
            FieldInfo[] fieldInfo = type.GetFields();
            foreach (var field in fieldInfo)
            {
                var attr = field.GetCustomAttribute<SkillTargetAttribute>();
                if (attr != null)
                {
                    
                    skillFields.Enqueue(field);
                }
            }

        }

        public void RemoveMode()
        {
            // 스킬 입력이 불완전한 상태로 입력 될 경우 초기화 시행
            if (!IsFieldEmpty() || currentField != null)
            {
                CancelSkillInput();
            }
            _inputManager.OnObjectClicked.RemoveListener(SetClick);

            _inputManager.UI.cancelButton.gameObject.SetActive(false);
            _inputManager.UI.cancelButton.onClick.RemoveAllListeners();
        }

        public void SetMode()
        {
            _inputManager.OnObjectClicked.AddListener(SetClick);

            _inputManager.UI.cancelButton.gameObject.SetActive(true);
            _inputManager.UI.cancelButton.onClick.AddListener(()=> {
                _inputManager.SetInputMode(phaseService.CurPhase);
                });

            Debug.Log("SkillMode");
            // 첫 번째 스킬 입력 필드 설정
            SetNextField();
        }

        
        void SetClick(GameObject obj)
        {
            var component = obj.GetComponent(currentField.FieldType);
            currentField.SetValue(skill, component);
            if (skill.IsValidInput(currentField))
            {
                GameObject selecter = UnityEngine.Object.Instantiate(_inputManager.skillSelecter);
                selecters.Add(selecter);
                selecter.transform.position = obj.transform.position + Vector3.up * 0.1f;

                SetNextField();
            }
        }
        bool IsFieldEmpty()
        {
            if (skillFields.Count <= 0)
            {
                return true;
            }
            else return false;
        }
        void SetNextField()
        {
            if (!IsFieldEmpty())
            {
                currentField = skillFields.Dequeue();
                if (currentField.GetValue(skill) != null)
                {
                    SetNextField();
                    return;
                }
                var attr = currentField.GetCustomAttribute<SkillTargetAttribute>();
                if (attr != null)
                {
                    Debug.Log(attr.text);
                }
            }
            else
            {
                currentField = null;
                _inputManager.controller.CreateCommand(skill, selecters.ToArray());
            }

        }
        void CancelSkillInput()
        {
            foreach (var obj in selecters)
            {
                GameObject.Destroy(obj);
            }
            selecters.Clear();
            Debug.Log("스킬 비정상적 종료로 인한 리셋");
            //skill.Reinitialize();
        }

        #region newSkillInput
        
        // 스킬 입력 및 커맨드 생성 코루틴
        public IEnumerator InputSkill(SkillComponent skill)
        {
            // TODO : skill.inputSkill 실행
            // TODO : 정상 완료 시 커맨드 생성 및 스킬 입력 모드 종료
            yield break;
        }

        public IEnumerator InputEntity(List<Entity> list, Action<Entity> input, Action<bool> callback, int count = -1)
        {
            bool isContinue = true;
            for (int i = 0; i < Mathf.Min(count, list.Count); i++)
            {
                // TODO : list 기물 시각화
                // TODO : list 내에서 count만큼 입력 받기, 입력받은 기물은 선택 대상에서 제거
                // TODO : 시각화 제거
                if (!isContinue)
                {
                    callback?.Invoke(false);
                    yield break;
                }
            }

            callback?.Invoke(true);
            
        }
        public IEnumerator InputTile(List<Tile> list, Action<Tile> input, Action<bool> callback, int count = -1)
        {
            bool isContinue = true;
            for(int i=0;i < Mathf.Min(count,list.Count); i++)
            {
                // TODO : list 타일 시각화
                // TODO : list 내에서 count만큼 입력 받기, 입력받은 타일은 선택 대상에서 제거
                // TODO : 시각화 제거
                if (!isContinue)
                {
                    callback?.Invoke(false);
                    yield break;
                }
            }


            callback?.Invoke(true);
        }

        #endregion
    }
}
