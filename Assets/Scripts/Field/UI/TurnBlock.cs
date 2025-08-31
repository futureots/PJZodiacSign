using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TurnBlock : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI turnText;
    public Image backgroundImage;
    
    private ITurn turnData;
    Animator anim;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        GetComponentInChildren<CanvasGroup>().DOFade(1, 1f);
        //anim.SetTrigger("Appear");
    }
    /// <summary>
    /// TurnBlock을 초기화하고 ITurn 데이터를 설정합니다.
    /// </summary>
    /// <param name="turn">표시할 턴 데이터</param>
    /// <param name="team">팀 번호 (1 또는 2)</param>
    public void Initialize(ITurn turn)
    {
        turnData = turn;
        
        
        UpdateDisplay();
    }
    
    /// <summary>
    /// 턴 타입과 팀에 따라 UI를 업데이트합니다.
    /// </summary>
    private void UpdateDisplay()
    {
        if (turnData == null) return;

        // 턴 타입에 따른 텍스트 설정
        string text = GetTurnText();

        // 전체 텍스트 조합 (예: "Team 1 행동", "Team 2 공격")
        turnText.text = text;
        
        // 팀에 따른 색상 설정
        Color teamColor = GameManager.Instance.teamColorTable.teamColors[turnData.Agent.team.teamNumber];

        // 배경색을 팀 색상으로, 텍스트 색상을 턴 타입 색상으로 설정
        if (backgroundImage != null)
        {
            backgroundImage.color = teamColor;
        }
    }
    
    /// <summary>
    /// 턴 타입에 따른 텍스트를 반환합니다.
    /// </summary>
    string GetTurnText()
    {
        string text = "";
        text += "팀 " + turnData.Agent.team.teamNumber;
        if (turnData is ActionTurn)
        {
            text += " 행동";
        }
        else if (turnData is AttackTurn)
        {
            text += " 공격";
        }
        return text;
    }
    
    /// <summary>
    /// 현재 저장된 ITurn 데이터를 반환합니다.
    /// </summary>
    public ITurn GetTurnData()
    {
        return turnData;
    }

    public void Destroy()
    {
        //anim.SetTrigger("Disappear");
        GetComponentInChildren<CanvasGroup>().DOFade(0, 1f);
        Destroy(gameObject, 1f);
    }

}

