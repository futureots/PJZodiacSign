using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountUI : InputManagerUI
{
    InputManager _inputManager;
    // 25턴 시작 시 버튼 활성화
    [SerializeField] private Button drawButton;

    [SerializeField] private GameObject turnPanel;
    [SerializeField] private TextMeshProUGUI turnCountText;
    [SerializeField] private CalculateUI calcPanel;

    private int drawCount;
    
    // 50턴 시작 시 UI에서 계산 및 종료하기
    public override void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        var fieldController = _inputManager.agent.fieldController;
        fieldController.OnPhaseStarted += PhaseStart;
        fieldController.OnTurnStarted += TurnStart;
        fieldController.OnDraw += SetDraw;
        drawCount = fieldController.TurnLimit;
        
        drawButton.gameObject.SetActive(false);
        turnPanel.SetActive(false);
        calcPanel.gameObject.SetActive(false);
    }

    void PhaseStart(Phase phase)
    {
        if(phase.phaseName == PhaseType.Battle) turnPanel.SetActive(true);
        else turnPanel.SetActive(false);
    }
    
    void TurnStart(Turn turn, uint count)
    {
        turnCountText.text = $"{count}/{drawCount}";
        // 턴 시작 시 활성화
        if (count == drawCount/2)
        {
            drawButton.gameObject.SetActive(true);
        }
        
    }

    // 무승부 선택 시 실행하는 함수
    public void SetDraw()
    {
        calcPanel.gameObject.SetActive(true);
        StartCoroutine(calcPanel.Calculate(_inputManager));
    }
}
