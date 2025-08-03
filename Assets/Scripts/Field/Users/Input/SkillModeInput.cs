using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace PlayerInput
{
    public class SkillModeInput : IModeInput
    {
        IActive skill;
        Action<InputAction.CallbackContext> bindAction;

        InputManager _inputManager;
        public SkillModeInput(InputManager input, IActive skill)
        {
            this.skill = skill;
            _inputManager = input;
            // 스킬의 변수 중에 입력이 필요한 값만 큐에 저장
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
            Debug.Log("RemoveSkillMode");

            _inputManager.OnObjectClicked.RemoveListener(SetClick);
        }

        public void SetMode()
        {
            _inputManager.OnObjectClicked.AddListener(SetClick);

            // 첫번째 스킬 입력값 설정
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
                var attr = currentField.GetCustomAttribute<SkillTargetAttribute>();
                if (attr != null)
                {
                    Debug.Log(attr.text);
                }
            }
            else
            {
                _inputManager.controller.CreateCommand(skill, selecters.ToArray());
                _inputManager.SetInputMode(Mode.Move);
            }
        }
    }
}
