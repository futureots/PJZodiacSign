using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{


    Queue<ITurn> turns;
    bool isTurnEnd;

    private void Awake()
    {
        turns = new Queue<ITurn>();
    }
    private void Start()
    {

        turns.Enqueue(new RepairTurn());
        AddTurnCycle();
        
        StartTurn();
    }
    void StartTurn()
    {
        var curTurn = turns.Dequeue();
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
        var list = new Queue<ITurn>();
        foreach (var ctrler in GameManager.Instance.controllers)
        {
            AddTeamTurn(ctrler,list);
        }
        list.Enqueue(list.Dequeue());
        while (list.Count > 0)
        {
            turns.Enqueue(list.Dequeue());
        }
    }
    public void AddTeamTurn(EntityController controller, Queue<ITurn> queue)
    {
        queue.Enqueue(new AttackTurn(controller));
        queue.Enqueue(new ActionTurn(controller));
    }
}