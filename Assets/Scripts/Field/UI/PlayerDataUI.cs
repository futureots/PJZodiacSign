using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    public Agent agent;

    public TextMeshProUGUI credit;
    public TextMeshProUGUI level;

    public void Init(Agent agent)
    {
        this.agent = agent;
        //agent.onCreditChanged += UpdateCredit;
        //GameManager.onNextLevel += UpdateLevel;
    }


    public void UpdateCredit(int credit)
    {
        this.credit.text = credit.ToString();
    }

    public void UpdateLevel(int level)
    {
        this.level.text = level.ToString();
    }

    
    private void OnDisable()
    {
        //agent.onCreditChanged -= UpdateCredit;
        //GameManager.onNextLevel -= UpdateLevel;
    }
}
