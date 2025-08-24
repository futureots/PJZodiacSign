using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace PlayerInput
{
    public class SkillModeInput : IModeInput
    {
        Mode prevMode;

        IActive skill;
        Action<InputAction.CallbackContext> bindAction;

        InputManager _inputManager;
        public SkillModeInput(InputManager input, IActive skill, Mode mode)
        {
            prevMode = mode;
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
                _inputManager.SetInputMode(prevMode);
                });

            Debug.Log("SkillMode");
            // 첫 번째 스킬 입력 필드 설정
            SetNextField();
        }

        Queue<FieldInfo> skillFields;
        FieldInfo currentField;
        List<GameObject> selecters;
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
    }
}
