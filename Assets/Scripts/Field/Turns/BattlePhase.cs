using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : IPhase
{
    // 전투 턴 큐(중간 삽입도 가능하도록 LinkedList 사용)
    private LinkedList<ITurn> turns;
    private Action onPhaseEnd;
    
    public BattlePhase()
    {
        turns = new LinkedList<ITurn>();
        AddBattleTurns();
    }
    
    private void AddBattleTurns()
    {
        // 각 에이전트마다 ActionTurn과 AttackTurn을 추가
        foreach (var agent in GameManager.Instance.agents)
        {
            turns.AddLast(new ActionTurn(agent));
            turns.AddLast(new AttackTurn(agent));
        }
    }
    
    public void StartPhase(Action onPhaseEnd)
    {
        GameManager.Instance.SetEntityHpBar();

        this.onPhaseEnd = onPhaseEnd;
        Debug.Log("전투 페이즈 시작");
        PhaseManager.curPhase = PhaseType.Battle;
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetBattlePhase();
        }
        StartNextTurn();
    
    }
    
    private void StartNextTurn()
    {
        // 일반적으로 실행되지 않아야 함.
        if (turns.Count <8)
        {
            AddBattleTurns();
        }
        
        var currentTurn = turns.First.Value;
        turns.RemoveFirst();
        
        currentTurn.StartTurn(OnCombatTurnComplete);
    }
    
    // 턴 종료 시 사망한 기물 정리 및 승패 확인
    private void OnCombatTurnComplete()
    {
        GameManager.Instance.field.RemoveDeadEntities();

        //bool isEnd = GameManager.Instance.CheckGameEnd();

        if(GameManager.Instance.HasGameEnded(out Agent winner))
        {
            foreach (var agent in GameManager.Instance.agents)
            {
                agent.EndBattlePhase();
            }

            if (!GameManager.Instance.HandleBattleVictory(winner))
            {
                // 페이즈 종료
                onPhaseEnd?.Invoke();
            }
        }
        else
        {
            // 다음 전투 턴 시작
            StartNextTurn();
        }
    }
}
