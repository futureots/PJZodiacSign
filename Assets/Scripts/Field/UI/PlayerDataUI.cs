using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    private Agent _agent;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI creditText;
    public TextMeshProUGUI levelText;
    
    public void Init(Agent agent)
    {
        _agent = agent;
        
        // 크레딧 갱신
        agent.OnCreditChanged += UpdateCredit;
        UpdateCredit(agent.Credit);
        
        // level 갱신
        UpdateLevel(GameManager.Instance.Level);

        // 시간 갱신
        StageManager.Instance.timer.onTimerUpdate += UpdateTime;
        UpdateTime(StageManager.Instance.timer.GetTime());
    }


    public void UpdateCredit(int credit)
    {
        creditText.text = credit.ToString();
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void UpdateTime(int time)
    {
        // TODO : 초 값을 시간으로 변형 필요
        var second = time % 60;
        var minute = time / 60;
        
        timeText.text = $"{minute:D2} : {second:D2}";
    }
    
}
