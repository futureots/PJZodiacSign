using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BattlePhase : IPhase
{
    Field field;
    // 전투 턴 큐(중간 삽입도 가능하도록 LinkedList 사용)
    private LinkedList<ITurn> turns;
    private Action onPhaseEnd;

    // 턴 종료 시 턴 정보(행동 턴은 해당 커맨드 정보를 가짐) 반환 및 이벤트 추가(해당 턴 동안 기물 사망 시 로그 추가)
    
    public static Action<ITurn> OnTurnStarted;
    public int Level
    {
        get
        {
            return level;
        }
    }

    public PhaseType PhaseType => PhaseType.Battle;

    int level;

    public BattlePhase(int level, Field mainField)
    {
        this.level = level;
        field = mainField;
        turns = new LinkedList<ITurn>();
        
    }
    
    private void AddBattleTurns()
    {
        // // 각 에이전트마다 ActionTurn과 AttackTurn을 추가
        // foreach (var agent in GameManager.Instance.agents)
        // {
        //     var actionTurn = new ActionTurn(agent);
        //     turns.AddLast(actionTurn);
        //     
        //     var attackTurn = new AttackTurn(GameManager.Instance.GetOppositeAgent(agent));
        //     turns.AddLast(attackTurn);
        //
        //     // 현재 attackTurn내부에 해당 함수 실행 중 필요 시 사용(굳이 없을 듯)
        //     /*
        //     // 중립 오브젝트 공격 턴 추가
        //     var calcTurn = new CalculationTurn();
        //     turns.AddLast(calcTurn);
        //     */
        // }
    }
    
    public void StartPhase(Action onPhaseEnd)
    {
        // GameManager.Instance.SetEntityHpBar();
        //
        // this.onPhaseEnd = onPhaseEnd;
        // Debug.Log("전투 페이즈 시작");
        // foreach (var agent in GameManager.Instance.agents)
        // {
        //     agent.SetBattlePhase();
        // }
        // StartNextTurn();
    
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
        OnTurnStarted(currentTurn);
        
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
        
        
    }
    
    
    // 턴 종료 시 사망한 기물 정리 및 승패 확인
    private void OnCombatTurnComplete()
    {
        // field.RemoveDeadEntities();
        //
        // //bool isEnd = GameManager.Instance.CheckGameEnd();
        //
        // if(GameManager.Instance.HasGameEnded(out Agent winner))
        // {
        //     foreach (var agent in GameManager.Instance.agents)
        //     {
        //         agent.EndBattlePhase();
        //     }
        //
        //     if (!GameManager.Instance.HandleBattleVictory(winner))
        //     {
        //         // 페이즈 종료
        //         onPhaseEnd?.Invoke();
        //     }
        // }
        // else
        // {
        //     // 다음 전투 턴 시작
        //     StartNextTurn();
        // }
    }
}
