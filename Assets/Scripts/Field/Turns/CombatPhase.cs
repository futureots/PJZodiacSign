using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPhase : IPhase
{
    // 전투 턴 큐(중간 삽입도 가능하도록 LinkedList 사용)
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
        // 일반적으로 실행되지 않아야 함.
        if (combatTurns.Count == 0)
        {
            Debug.Log("Anomaly End Combat");
            onPhaseEnd?.Invoke();
            return;
        }
        
        var currentTurn = combatTurns.First.Value;
        combatTurns.RemoveFirst();
        
        currentTurn.StartTurn(OnCombatTurnComplete);
    }
    
    // 턴 종료 시 사망한 기물 정리 및 승패 확인
    private void OnCombatTurnComplete()
    {
        GameManager.Instance.field.CleanField();
        bool isEnd = GameManager.Instance.CheckGameEnd();
        
        if (isEnd)
        {
            // 게임이 끝나면 페이즈 종료
            onPhaseEnd?.Invoke();
        }
        else
        {
            // 다음 전투 턴 시작
            StartNextCombatTurn();
        }
    }
}
