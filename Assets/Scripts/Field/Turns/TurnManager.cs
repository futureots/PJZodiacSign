using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{

    public GameInputState currentState { get; private set; } = GameInputState.Planning;

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
public enum GameInputState
{
    Planning,//명령 입력
    Executing,//입력 무시
    Inspecting//명령 무시, 유닛 정보 표시만 가능
}