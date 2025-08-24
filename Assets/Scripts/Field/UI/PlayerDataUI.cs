using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    public Agent agent;

    public TextMeshProUGUI credit;
    public TextMeshProUGUI level;
    private void Awake()
    {
        agent = transform.root.GetComponent<Agent>();
    }

    private void Start()
    {
        
        //UpdateCredit(agent.Credit);
    }


    public void UpdateCredit(int credit)
    {
        this.credit.text = credit.ToString();
    }

    public void UpdateLevel(int level)
    {
        this.level.text = level.ToString();
    }

    private void OnEnable()
    {
        agent.onCreditChanged += UpdateCredit;
        GameManager.onNextLevel += UpdateLevel;
        
    }
    private void OnDisable()
    {
        agent.onCreditChanged -= UpdateCredit;
        GameManager.onNextLevel -= UpdateLevel;
    }
}
