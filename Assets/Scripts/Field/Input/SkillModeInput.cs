using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillModeInput : IModeInput
{
    IActive skill;
    GameInputActions _inputAction;
    Action<InputAction.CallbackContext> bindAction;
    InputManager _inputManager
    {
        get
        {
            return InputManager.Instance;
        }
    }

    public SkillModeInput(GameInputActions inputAction,IActive skill)
    {
        this.skill = skill;
        _inputAction = inputAction;

        // 스킬의 변수 중에 입력이 필요한 값만 큐에 저장
        selecters = new();
        skillFields = new();
        var type = skill.GetType();
        FieldInfo[] fieldInfo = type.GetFields();
        foreach (var field in fieldInfo)
        {
            var attr = (SkillTargetAttribute)field.GetCustomAttribute(typeof(SkillTargetAttribute));
            if (attr != null)
            {
                skillFields.Enqueue(field);
            }
        }

    }

    public void RemoveMode()
    {
        Debug.Log("RemoveSkillMode");
        _inputAction.Gameplay.Click.started -= SetClick;
    }

    public void SetMode()
    {
        _inputAction.Gameplay.Click.started += SetClick;

        // 첫번째 스킬 입력값 설정
        SetNextField();
    }

    Queue<FieldInfo> skillFields;
    FieldInfo currentField;
    List<GameObject> selecters;
    void SetClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.PointerPosition);
        // 부딪힌 기물, (타일) UI 표시 
        if (Physics.Raycast(ray, out var hit))
        {
            var obj = hit.collider.gameObject;
            var component = obj.GetComponent(currentField.FieldType);
            currentField.SetValue(skill, component);
            if (skill.IsValidInput(currentField))
            {
                GameObject selecter = UnityEngine.Object.Instantiate(_inputManager.skillSelecter);
                selecters.Add(selecter);
                selecter.transform.position = obj.transform.position + Vector3.up * 0.1f;

                SetNextField();
            }
        };
    }
    bool IsFieldEmpty()
    {
        Debug.Log($"SkillField Count : {skillFields.Count}");
        if(skillFields.Count <= 0)
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
        }
        else
        {
            _inputManager.controller.CreateCommand(skill, selecters.ToArray());
            _inputManager.SetInputMode(InputManager.Mode.None);
        }
    }
}
