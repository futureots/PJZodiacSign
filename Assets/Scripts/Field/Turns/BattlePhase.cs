using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : IPhase
{
    // 전투 턴 큐(중간 삽입도 가능하도록 LinkedList 사용)
    private LinkedList<ITurn> turns;
    private Action onPhaseEnd;

    // 턴 큐 변경 이벤트
    public static event EventHandler<TurnQueueEventArgs> OnTurnQueueChanged;

    public int Level
    {
        get
        {
            return level;
        }
    }
    int level;

    public BattlePhase(int level)
    {
        this.level = level;
        turns = new LinkedList<ITurn>();
        
    }
    
    private void AddBattleTurns()
    {
        // 각 에이전트마다 ActionTurn과 AttackTurn을 추가
        foreach (var agent in GameManager.Instance.agents)
        {
            var actionTurn = new ActionTurn(agent);
            turns.AddLast(actionTurn);
            OnTurnQueueChanged?.Invoke(this, new TurnQueueEventArgs(actionTurn, TurnQueueEventType.TurnAdded));
            
            var attackTurn = new AttackTurn(agent);
            turns.AddLast(attackTurn);
            OnTurnQueueChanged?.Invoke(this, new TurnQueueEventArgs(attackTurn, TurnQueueEventType.TurnAdded));
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
        //Debug.Log("Start Next Turn");
        if (turns.Count <5)
        {
            AddBattleTurns();
        }
        
        var currentTurn = turns.First.Value;
        
        turns.RemoveFirst();
        // 턴 시작 이벤트 발생
        OnTurnQueueChanged?.Invoke(this, new TurnQueueEventArgs(currentTurn, TurnQueueEventType.TurnStarted));
        
        currentTurn.StartTurn(OnCombatTurnComplete);
    }
    
    /// <summary>
    /// 특정 위치에 턴을 삽입합니다.
    /// </summary>
    /// <param name="turn">삽입할 턴</param>
    /// <param name="index">삽입할 위치 (0부터 시작)</param>
    public void InsertTurn(ITurn turn, int index)
    {
        if (index < 0 || index > turns.Count)
        {
            Debug.LogWarning($"Invalid index: {index}. Index should be between 0 and {turns.Count}");
            return;
        }
        
        var node = turns.First;
        for (int i = 0; i < index; i++)
        {
            node = node.Next;
        }
        
        if (node == null)
        {
            turns.AddLast(turn);
        }
        else
        {
            turns.AddBefore(node, turn);
        }
        
        // 이벤트 발생
        OnTurnQueueChanged?.Invoke(this, new TurnQueueEventArgs(turn, TurnQueueEventType.TurnInserted, index));
    }
    
    /// <summary>
    /// 특정 턴을 제거합니다.
    /// </summary>
    /// <param name="turn">제거할 턴</param>
    public void RemoveTurn(ITurn turn)
    {
        if (turns.Remove(turn))
        {
            OnTurnQueueChanged?.Invoke(this, new TurnQueueEventArgs(turn, TurnQueueEventType.TurnRemoved));
        }
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
