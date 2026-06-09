using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class StageEndUI : InputManagerUI
{
    InputManager _inputManager;
    [SerializeField] private GameObject blindPanel;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button defeatButton;
    [SerializeField] private Button endButton;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI pointText;
    
    [Header("Localization")]
    [SerializeField] private LocalizedString victoryString;
    [SerializeField] private LocalizedString defeatString;
    [SerializeField] private LocalizedString endString;
    [SerializeField] private LocalizedString stageTimeString;
    [SerializeField] private LocalizedString stagePointString;
    [SerializeField] private LocalizedString totalTimeString;
    [SerializeField] private LocalizedString totalPointString;
    
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
                    title.text = $"{victoryString.GetLocalizedString()}!";
                    // 승리 시에만 다음 버튼 활성화
                    continueButton.gameObject.SetActive(true);
                    defeatButton.gameObject.SetActive(true);
                    endButton.gameObject.SetActive(false);
                    SetStageValue();
                }
                else
                {
                    // TODO : 랭킹 서버 만들거면 여기서 데이터 보내기
                    title.text = $"{endString.GetLocalizedString()}!";
                    continueButton.gameObject.SetActive(false);
                    defeatButton.gameObject.SetActive(false);
                    endButton.gameObject.SetActive(true);
                    SetLoopValue();
                }
                
            }
            else // 패배 시
            {
                title.text = $"{defeatString.GetLocalizedString()}...";
                continueButton.gameObject.SetActive(false);
                defeatButton.gameObject.SetActive(true);
                endButton.gameObject.SetActive(false);
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
        timeText.text = $"{stageTimeString.GetLocalizedString()} : {minute:D2} : {second:D2}";
            
        // 점수 표시
        var point =  StageManager.Instance.point;
        pointText.text = $"{stageTimeString.GetLocalizedString()} : {point}";
    }

    // 전체 루프에 대한 정보 출력
    void SetLoopValue()
    {
        // 총 시간 표시
        var time = StageManager.Instance.timer.GetTime();
        var second = time % 60;
        var minute = time / 60;
        timeText.text = $"{totalTimeString.GetLocalizedString()} : {minute:D2} : {second:D2}";
            
        // 점수 표시
        var point = StageManager.Instance.GetTotalPoint();
        pointText.text = $"{totalPointString.GetLocalizedString()} : {point}";
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
