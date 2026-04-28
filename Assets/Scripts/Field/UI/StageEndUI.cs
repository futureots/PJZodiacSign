using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageEndUI : InputManagerUI
{
    public bool isTutorial;
    InputManager _inputManager;
    [SerializeField] private GameObject blindPanel;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button defeatButton;
    [SerializeField] private Button endButton;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI pointText;
    
    public override void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        StageManager.Instance.OnStageEnded += id =>
        {
            blindPanel.SetActive(true);
            panel.SetActive(true);
            
            if (id == Agent.LocalPlayer.id) // 승리 시
            {
                if (!StageManager.Instance.isLastLevel)
                {
                    title.text = "클리어!";
                    // 승리 시에만 다음 버튼 활성화
                    continueButton.gameObject.SetActive(true);
                    SetStageValue();
                }
                else
                {
                    // TODO : 랭킹 서버 만들거면 여기서 데이터 보내기
                    title.text = "루프 완료!";
                    defeatButton.gameObject.SetActive(false);
                    endButton.gameObject.SetActive(true);
                    SetLoopValue();
                }
                
            }
            else // 패배 시
            {
                SetLoopValue();
            }
        };
    }

    // 현재 스테이지 대한 정보만 출력
    void SetStageValue()
    {
        // 시간 표시
        var time = StageManager.Instance.timer.GetElapsedTime();
        var second = time % 60;
        var minute = time / 60;
        timeText.text = $"걸린 시간 : {minute:D2} : {second:D2}";
            
        // 점수 표시
        var point =  StageManager.Instance.point;
        pointText.text = $"획득 점수 : {point}";
    }

    // 전체 루프에 대한 정보 출력
    void SetLoopValue()
    {
        // 총 시간 표시
        var time = StageManager.Instance.timer.GetTime();
        var second = time % 60;
        var minute = time / 60;
        timeText.text = $"플레이 시간 : {minute:D2} : {second:D2}";
            
        // 점수 표시
        var point = StageManager.Instance.GetTotalPoint();
        pointText.text = $"전체 점수 : {point}";
    }
    

    public void ContinueGame()
    {
        _inputManager.agent.fieldController.ContinueGame();
    }

    public void DefeatGame()
    {
        _inputManager.agent.fieldController.EndGame();
    }
}
