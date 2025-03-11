using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPanel : MonoBehaviour
{
    public TurnIndicator panelPrefab;
    Queue<TurnIndicator> turn;
    Queue<int> latterTurn;

    private void Start()
    {
        GameManager.Instance.endTurn += DeQueue;
        turn = new Queue<TurnIndicator>();
        latterTurn = new Queue<int>();
        for(int i = 0; i <= 2; i++)
        {
            EnQueue(i);
        }
    }

    // 디버그용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DeQueue();
        }
    }

    public void EnQueue(int num)
    {
        if (turn.Count > 2)
        {
            latterTurn.Enqueue(num);
            return;
        }
        AddQueue(num);
    }
    void AddQueue(int num)
    {
        var obj = Instantiate(panelPrefab, transform);
        var indicator = obj.GetComponent<TurnIndicator>();
        indicator.Constructor(num);
        turn.Enqueue(indicator);
        if (num == 2)
        {
            EnQueue(3);
        }
    }
    public void DeQueue()
    {
        if (turn.Count <= 0) return;
        var obj = turn.Dequeue();
        Destroy(obj.gameObject);
        if (latterTurn.Count <= 0) return;
        int next = latterTurn.Dequeue();
        AddQueue(next);
    }
}
