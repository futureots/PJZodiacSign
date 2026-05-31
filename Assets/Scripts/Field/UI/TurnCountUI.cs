using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountUI : InputManagerUI
{
    // 25턴 시작 시 버튼 활성화
    [SerializeField] private Button drawButton;
    [SerializeField] private TextMeshProUGUI turnCountText;
    
    // 50턴 시작 시 UI에서 계산 및 종료하기
    public override void Init(InputManager inputManager)
    {
        inputManager.agent.fieldController.OnTurnStarted += TurnStart;
    }

    void TurnStart(Turn turn, uint count)
    {
        
    }
}
