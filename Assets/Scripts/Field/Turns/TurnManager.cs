using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    
    Queue<ITurn> turns;
    bool isTurnEnd;

    public void AddTurn(ITurn turn)
    {
        turns.Enqueue(turn);
    }
    private void Awake()
    {
        turns = new Queue<ITurn>();
    }
    private void Start()
    {
        turns.Enqueue(new ActionTurn());
        turns.Enqueue(new ActionTurn());
        turns.Enqueue(new AttackTurn());
        StartTurn();
    }
    void StartTurn()
    {
        var curTurn = turns.Dequeue();
        if (turns.Count < 4)
        {
            turns.Enqueue(new ActionTurn());
            turns.Enqueue(new ActionTurn());
            turns.Enqueue(new AttackTurn());
        }
        //Debug.Log($"Current TurnCount : {turns.Count}");
        curTurn.Execute(OnTurnComplete);
    }

    void OnTurnComplete()
    {

        Field.Instance.CleanField();
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
}
