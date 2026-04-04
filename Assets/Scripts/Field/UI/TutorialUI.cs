using DG.Tweening;
using PlayerInput;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    InputManager _inputManager;

    private void Start()
    {
        if (TryGetComponent(out RectTransform rect))
        {
            // 위치 이동은 스크립트 상으로 이동 가능함
            //rect.DOAnchorPos(Vector2.left*2560, 1f).SetEase(Ease.Linear);
        }
    }

    public GameObject repairTutorial;
    public GameObject battleTutorial;
    
    [SerializeField] TextMeshProUGUI description;
    [SerializeField, TextArea(3,5)] string repairDescription;
    [SerializeField, TextArea(3, 5)] string moveDescription;
    [SerializeField, TextArea(3, 5)] string skillDescription;
    [SerializeField, TextArea(3, 5)] string defaultDescription;

    public void Init(InputManager inputManager)
    {
        // TODO : 메인으로 이동하는 버튼 항시 표시(튜토리얼 스킵 버튼)
        
        
        _inputManager = inputManager;
        _inputManager.agent.fieldController.OnPhaseStarted += OnPhaseChange;
        _inputManager.OnModeChanged += OnModeChange;
        
    }

    void OnPhaseChange(Phase phase)
    {
        // TODO : 정비 페이즈일 경우 정비 설명 UI 표시(전체 화면, 패널 외 상호작용 불가, 모든 패널 확인 시 비활성화)
        // TODO : 전투 페이즈일 경우 전투 설명 UI 표시(위와 동일)
    }

    void OnModeChange(IInputState state)
    {
        string text = state switch
        {
            RepairModeInput => repairDescription,
            MoveModeInput => moveDescription,
            SkillModeInput => skillDescription,
            _ => defaultDescription
        };
        description.text = text;
    }
}
