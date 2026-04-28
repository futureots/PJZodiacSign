using System;
using TMPro;
using UnityEngine;

public class TutorialUI : InputManagerUI
{
    InputManager _inputManager;

    public TutorialPanel repairTutorial;
    public TutorialPanel battleTutorial;

    public override void Init(InputManager inputManager)
    {
        // TODO : 메인으로 이동하는 버튼 항시 표시(튜토리얼 스킵 버튼)
        _inputManager = inputManager;
        _inputManager.agent.fieldController.OnPhaseStarted += OnPhaseChange;
    }

    void OnPhaseChange(Phase phase)
    {
        switch (phase.phaseName)
        {
            // TODO : 정비 페이즈일 경우 정비 설명 UI 표시(전체 화면, 패널 외 상호작용 불가, 모든 패널 확인 시 비활성화)
            case PhaseType.Repair:
                repairTutorial.gameObject.SetActive(true);
                battleTutorial.gameObject.SetActive(false);
                repairTutorial.Init();
                break;
            // TODO : 전투 페이즈일 경우 전투 설명 UI 표시(위와 동일)
            case PhaseType.Battle:
                repairTutorial.gameObject.SetActive(false);
                battleTutorial.gameObject.SetActive(true);
                battleTutorial.Init();
                break;
        }
    }
}
