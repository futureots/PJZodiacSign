using PlayerInput;
using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    private Agent _agent;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI creditText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI entityCountText;
    
    public void Init(InputManager inputManager)
    {
        _agent = inputManager.agent;
        
        // 크레딧 갱신
        _agent.OnCreditChanged += UpdateCredit;
        UpdateCredit(_agent.Credit);
        
        // level 갱신
        UpdateLevel(GameManager.Instance.Level);

        // 시간 갱신
        StageManager.Instance.timer.onTimerUpdate += UpdateTime;
        UpdateTime(StageManager.Instance.timer.GetTime());
        
        // 배치 기물 수 갱신
        inputManager.OnModeChanged += (state) =>
        {
            if (state is RepairModeInput repair)
            {
                repair.onEntityCountChanged += UpdateEntityCount;
            }
        };
    }

    public void UpdateEntityCount(int count,int max)
    {
        entityCountText.text = $"{count}/{max}";
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
