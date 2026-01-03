using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TurnBlock : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI turnText;
    public Image backgroundImage;
    
    private Turn turnData;
    Animator anim;
    public List<LogBlock> logs;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        foreach (var block in logs)
        {
            Destroy(block.gameObject);
        }
        logs.Clear();
    }

    /// <summary>
    /// TurnBlock을 초기화하고 ITurn 데이터를 설정합니다.
    /// </summary>
    /// <param name="turn">표시할 턴 데이터</param>
    /// <param name="team">팀 번호 (1 또는 2)</param>
    public void Initialize()
    {
        //turnData = turn;
        
        UpdateDisplay();

        // 해당 턴의 커맨드를 받는 구독자 생성 및 이벤트 발동 시 해당 커맨드에 대한 UI 업데이트 및 구독 해제
        //if(turnData is ActionTurn actionTurn)
        //{
        //    actionTurn.onCommandExecuted += CommandUpdate;
        //}
    }

    /// <summary>
    /// 해당 턴이 행동턴일 때 커맨드를 표기하는 기능
    /// </summary>
    /// <param name="cmd"></param>
    void CommandUpdate(Command cmd)
    {
        // 해당 커맨드에 대한 설명이 포함된 블록 표시
        UpdateDisplay(cmd);
        //if (turnData is ActionTurn actionTurn)
        //{
        //    actionTurn.onCommandExecuted -= CommandUpdate;
        //}
    }
    /// <summary>
    /// 턴 타입과 팀에 따라 UI를 업데이트합니다.
    /// </summary>
    private void UpdateDisplay()
    {
        if (turnData == null) return;

        // 턴 타입에 따른 텍스트 설정
        //string text = GetTurnText();

        // 전체 텍스트 조합 (예: "Team 1 행동", "Team 2 공격")
        //turnText.text = text;
        
        // 팀에 따른 색상 설정
        // Color teamColor = GameManager.Instance.teamColorTable.teamColors[turnData.TeamNumber];
        // backgroundImage.color = teamColor;
    }

    void UpdateDisplay(Command cmd)
    {
        
        if(turnData == null) return;
        string text = "행동 불능";
        if (cmd != null)
        {
            text = cmd.ToString();
        }
        turnText.text = text;
    }

    



}

