using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    Agent agent;

    public TextMeshProUGUI credit;
    public TextMeshProUGUI level;
    private void Awake()
    {
        level.gameObject.SetActive(false);
    }
    public void Init(Agent agent)
    {
        this.agent = agent;
        agent.OnCreditChanged += UpdateCredit;
        UpdateCredit(agent.Credit);
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
