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
        GameManager.Instance.CheckGameEnd();
        StartTurn();
    }

    public void NextLevel(int level)
    {
        turns.Clear();
        turns.AddFirst(new RepairTurn(level));
    }

    void AddTurnCycle()
    {
        foreach (var ctrler in GameManager.Instance.agents)
        {
            // 플레이어 행동 후 플레이어 팀 외 기물 공격
            turns.AddLast(new ActionTurn(ctrler));
            turns.AddLast(new AttackTurn(ctrler));
        }
    }

}