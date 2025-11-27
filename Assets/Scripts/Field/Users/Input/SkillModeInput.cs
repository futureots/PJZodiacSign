using Condition;
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
        IPhaseManageService phaseService;

        IActive skill;
        
        Action<InputAction.CallbackContext> bindAction;

        InputManager _inputManager;

        List<GameObject> selecters;

        Queue<FieldInfo> skillFields;
        FieldInfo currentField;

        SkillComponent skillComp;
        Queue<ConditionData> conditions;
        ConditionData curCondition;
        ConditionArgs currentArgs, inputArgs;
        List<ConditionArgs> parameters;

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

            //parameters = new();
            //conditions = new();
            //foreach (var item in skillComp.skillData.conditions)
            //{
            //    conditions.Enqueue(item);
            //}
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

            //// 변경후 사용할 코드
            //switch (currentArgs.currentType)
            //{
            //    case ConditionType.TILE:
            //        var tile = obj.GetComponent<Tile>();
            //        if (currentArgs.tiles.Contains(tile))
            //        {
            //            inputArgs.tiles.Add(tile);
            //            if (inputArgs.tiles.Count >= curCondition.inputCount)
            //            {
            //                parameters.Add(inputArgs);
            //                SetNextField();
            //            }
            //        }
            //        break;
            //    case ConditionType.ENTITY:
            //        var entity = obj.GetComponent<Entity>();
            //        if (currentArgs.entities.Contains(entity))
            //        {
            //            inputArgs.entities.Add(entity);
            //            if(inputArgs.entities.Count >= curCondition.inputCount)
            //            {
            //                parameters.Add(inputArgs);
            //                SetNextField();
            //            }
            //        }
            //        break;
            //    default:
            //        break;
            //}
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

            //// 변경 후 사용할 코드
            //// 조건 하나 필터링하기
            //if (conditions.Count > 0)
            //{
            //    var curCondition = conditions.Dequeue();
            //    // default 주입
            //    currentArgs = new();
            //    if (curCondition.args.Count > 0)
            //    {
            //        var type = curCondition.args[0].conditionType;
            //        // 첫번째 타입에 대한 의존성 주입
            //    }
            //    // currentArgs에 현재 선택가능한 리스트 필터링
            //    foreach (var condition in curCondition.args)
            //    {
            //        condition.FilterConditions(currentArgs);
            //    }
            //    // 현재 curArg와 동일한 타입의 arg 생성
            //    inputArgs = new(currentArgs.currentType);
            //}
            //else
            //{
            //    // parameter와 skill을 담은 커맨드 생성 및 반환
            //}
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
