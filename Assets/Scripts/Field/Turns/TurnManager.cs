using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TurnManager : Singleton<TurnManager>
{


    public LinkedList<ITurn> turns;

    private void Awake()
    {
        turns = new LinkedList<ITurn>();
    }
    private void Start()
    {

        turns.AddLast(new RepairTurn());
        AddTurnCycle();
        
        StartTurn();
    }
    void StartTurn()
    {
        var curTurn = turns.First.Value;
        turns.RemoveFirst();
        if (turns.Count < 4)
        {
            AddTurnCycle();
        }
        //Debug.Log($"Current TurnCount : {turns.Count}");
        curTurn.Execute(OnTurnComplete);
    }

    void OnTurnComplete()
    {
        GameManager.Instance.field.CleanField();
        bool isEnd = GameManager.Instance.CheckGameEnd();
        if (isEnd)
        {
            turns.Clear();
            //모든 턴 정리 및 상호작용 제거
        }
        else
        {
            Debug.Log("Turn End");
            StartTurn();
        }
    }
    void AddTurnCycle()
    {
        foreach (var ctrler in GameManager.Instance.controllers)
        {
            // 플레이어 행동 후 플레이어 팀 외 기물 공격
            turns.AddLast(new ActionTurn(ctrler));
            turns.AddLast(new AttackTurn(ctrler));
        }
    }

}