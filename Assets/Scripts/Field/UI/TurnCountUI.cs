using System.Collections;
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
    [SerializeField] private GameObject calcPanel;
    [SerializeField] private GameObject valuePrefab;
    [SerializeField] private Transform teamValueTransform;
    [SerializeField] private int drawCount;
    
    // 50턴 시작 시 UI에서 계산 및 종료하기
    public override void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.agent.fieldController.OnPhaseStarted += PhaseStart;
        _inputManager.agent.fieldController.OnTurnStarted += TurnStart;
        
        drawButton.gameObject.SetActive(false);
        turnPanel.SetActive(false);
        calcPanel.SetActive(false);
    }

    void PhaseStart(Phase phase)
    {
        if(phase.phaseName == PhaseType.Battle) turnPanel.SetActive(true);
        else turnPanel.SetActive(false);
    }
    
    void TurnStart(Turn turn, uint count)
    {
        turnCountText.text = $"{count}/{drawCount*2}";
        // 25턴 시작 시 활성화
        if (count == drawCount)
        {
            drawButton.gameObject.SetActive(true);
        }
        
    }

    // 무승부 선택 시 실행하는 함수
    public void SetDraw()
    {
        calcPanel.SetActive(true);
        StartCoroutine(Calculate());
    }

    IEnumerator Calculate()
    {
        int myValue = 0;
        int opponentValue = 0;

        var field = StageManager.Instance.field;
        // 필드에 있는 모든 기물을 가져와서 값 계산
        foreach (var entity in field.GetEntities())
        {
            if (entity.team.teamNumber == _inputManager.agent.id)
            {
                myValue += entity.baseData.normalPrice * (entity.Level + 1);
            }
            else
            {
                opponentValue += entity.baseData.normalPrice * (entity.Level + 1);
            }

            yield return null;
        }
        
        // 승패 판별 후 종료 이벤트 실행 지금은 무조건 승리로 판별
        _inputManager.agent.fieldController.EndStage(Agent.LocalPlayer.id);
        
        yield break;
    }
}
