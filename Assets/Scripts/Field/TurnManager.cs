using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    
    public Queue<ITurn> turns;
    bool isTurnEnd;

    private void Start()
    {
        turns = new Queue<ITurn>();
        turns.Enqueue(new ActionTurn());
        turns.Enqueue(new ActionTurn());
        turns.Enqueue(new ActionTurn());
        StartTurn();
    }
    void StartTurn()
    {
        var curTurn = turns.Dequeue();
        Debug.Log($"CurTurnCount : {turns.Count}");
        curTurn.Execute(OnTurnComplete);
    }

    void OnTurnComplete()
    {
        Debug.Log("Turn End");
        StartTurn();
    }
}
