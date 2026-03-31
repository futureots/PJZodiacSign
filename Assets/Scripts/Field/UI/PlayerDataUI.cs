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
        agent.OnCreditChanged += UpdateCredit;
        UpdateCredit(agent.Credit);
        // TODO : level 표시, 현재 시간 표시(갱신 포함)
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
        timeText.text = time.ToString();
    }

    
    private void OnDisable()
    {
        //agent.onCreditChanged -= UpdateCredit;
        //GameManager.onNextLevel -= UpdateLevel;
    }
}
