using JetBrains.Annotations;
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
        // 턴 추가하기
        foreach (var ctrler in GameManager.Instance.controllers)
        {
            AddTeamTurn(ctrler);
        }
        //첫 번째 공격턴은 제거
        turns.Dequeue();
        StartTurn();
    }
    void StartTurn()
    {
        var curTurn = turns.Dequeue();
        if (turns.Count < 4)
        {
            foreach (var ctrler in GameManager.Instance.controllers)
            {
                AddTeamTurn(ctrler);
            }
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
    public void AddTeamTurn(EntityController controller)
    {
        turns.Enqueue(new AttackTurn(controller));
        turns.Enqueue(new ActionTurn(controller));
    }
}