using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TurnManager : MonoBehaviour
{
    public LinkedList<ITurn> turns;

    private void Awake()
    {
        turns = new LinkedList<ITurn>();
        GameManager.OnNextLevel += NextLevel;
    }

    public void StartTurn()
    {
        var curTurn = turns.First.Value;
        turns.RemoveFirst();
        if (turns.Count < 4)
        {
            AddTurnCycle();
        }
        //Debug.Log($"Current TurnCount : {turns.Count}");
        curTurn.StartTurn(OnTurnComplete);
    }

    void OnTurnComplete()
    {
        GameManager.Instance.field.CleanField();
        bool isEnd = GameManager.Instance.CheckGameEnd();

        if (!isEnd) StartTurn();
    }

    public void NextLevel(int level)
    {
        turns.Clear();
        turns.AddFirst(new RepairTurn(level));
        StartTurn();
    }

    void AddTurnCycle()
    {
        foreach (var ctrler in GameManager.Instance.agents)
        {
            // 에이전트 액션 턴과 에이전트 공격 턴 추가
            turns.AddLast(new ActionTurn(ctrler));
            turns.AddLast(new AttackTurn(ctrler));
        }
    }
}
