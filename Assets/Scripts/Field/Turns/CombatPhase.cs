using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPhase : IPhase
{
    private LinkedList<ITurn> combatTurns;
    private Action onPhaseEnd;
    
    public CombatPhase()
    {
        combatTurns = new LinkedList<ITurn>();
        InitializeCombatTurns();
    }
    
    private void InitializeCombatTurns()
    {
        // 각 에이전트마다 ActionTurn과 AttackTurn을 추가
        foreach (var agent in GameManager.Instance.agents)
        {
            combatTurns.AddLast(new ActionTurn(agent));
            combatTurns.AddLast(new AttackTurn(agent));
        }
    }
    
    public void StartPhase(Action onPhaseEnd)
    {
        this.onPhaseEnd = onPhaseEnd;
        Debug.Log("전투 페이즈 시작");
        StartNextCombatTurn();
    }
    
    private void StartNextCombatTurn()
    {
        if (combatTurns.Count == 0)
        {
            // 모든 전투 턴이 완료되면 승패 확인
            CheckGameResult();
            return;
        }
        
        var currentTurn = combatTurns.First.Value;
        combatTurns.RemoveFirst();
        
        currentTurn.StartTurn(OnCombatTurnComplete);
    }
    
    private void OnCombatTurnComplete()
    {
        GameManager.Instance.field.CleanField();
        bool isEnd = GameManager.Instance.CheckGameEnd();
        
        if (isEnd)
        {
            // 게임이 끝나면 승패 확인
            CheckGameResult();
        }
        else
        {
            // 다음 전투 턴 시작
            StartNextCombatTurn();
        }
    }
    
    // 승패 확인 메서드
    private void CheckGameResult()
    {
        // 승패 확인
        if (GameManager.Instance.IsGameEnd(out int winner))
        {
            Agent winAgent = null;
            for (int i = 0; i < GameManager.Instance.agents.Length; i++)
            {
                if (GameManager.Instance.agents[i].team.teamNumber == winner) 
                    winAgent = GameManager.Instance.agents[i];
            }
            
            if (winAgent != null && winAgent is InputManager)
            {
                Debug.Log("플레이어 승리! 다음 레벨로 진행합니다.");
                
                // 직접 다음 레벨로 진행
                GameManager.Instance.GoToNextLevel();
            }
            else
            {
                Debug.Log("플레이어 패배...");
                GameManager.Instance.GameEnd();
            }
        }
        else
        {
            // 게임이 끝나지 않았으면 현재 레벨의 전투 페이즈만 완료
            Debug.Log("전투 페이즈 완료");
        }
        
        // 전투 페이즈 종료
        onPhaseEnd?.Invoke();
    }
}
