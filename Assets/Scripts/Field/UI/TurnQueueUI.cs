using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

public class TurnQueueUI : MonoBehaviour
{
    public List<TurnBlock> list;
    public GameObject turnBlock;
    
    public Transform curTurn;
    public Transform nextTurns;
    public TurnBlock curTurnBlock;

    public float movingSpeed;
    // 제거된 턴들을 보관하는 리스트
    private List<ITurn> uncreatedTurns;
    

    private void Awake()
    {
        uncreatedTurns = new List<ITurn>();
    }
    
    /// <summary>
    /// BattlePhase에서 발생한 턴 큐 변경 이벤트를 처리합니다.
    /// </summary>
    private void OnTurnQueueChanged(object sender, TurnQueueEventArgs e)
    {
        switch (e.EventType)
        {
            case TurnQueueEventType.TurnAdded:
                PushBack(e.Turn);
                break;
            case TurnQueueEventType.TurnInserted:
                InsertTurnBlock(e.Index, e.Turn);
                break;
            case TurnQueueEventType.TurnStarted:
                PopFront();
                break;
            case TurnQueueEventType.TurnRemoved:
                // 특정 턴 제거 로직이 필요한 경우 구현
                break;
            case TurnQueueEventType.TurnMoved:
                // 턴 이동 로직이 필요한 경우 구현
                break;
        }
    }
    public int capacity;
    public void PushBack(ITurn turn)
    {
        if (list.Count >= capacity)
        {
            uncreatedTurns.Add(turn);
            return;
        }
        InsertTurnBlock(list.Count, turn);
    }
    
    public void PushFront(ITurn turn)
    {
        InsertTurnBlock(0, turn);
    }

    public void PopFront()
    {
        RemoveFrontBlock();
    }
    
    /// <summary>
    /// 특정 인덱스에 턴 블럭을 삽입하고 나머지 블럭들을 이동시킵니다.
    /// </summary>
    /// <param name="index">삽입할 위치 (0 ~ list.Count)</param>
    /// <param name="turn">삽입할 턴</param>
    public void InsertTurnBlock(int index, ITurn turn)
    {
        Debug.Log("InsertTurnBlock");
        if (list.Count >= capacity)
        {
            Debug.Log($"list {list.Count} ");
            RemoveBackBlock();
        }
        // 인덱스 범위 확인
        if (index < 0 || index > list.Count)
        {
            Debug.LogWarning($"Invalid index: {index}. Index should be between 0 and {list.Count}");
            return;
        }

        // 공간 만들기
        for (int i = index; i < list.Count; i++)
        {
            var block = list[i].transform;
            block.DOLocalMoveY(-10 + (i + 1 * -70), 1f);
        }

        // 새로운 블럭 생성
        var newBlock = Instantiate(turnBlock, nextTurns);
        newBlock.transform.localScale = Vector3.one * 0.8f;
        newBlock.transform.SetSiblingIndex(index);
        newBlock.GetComponent<RectTransform>().localPosition = new Vector3(0, -10 + index*-60, 0);
        // TurnBlock 컴포넌트 초기화
        var turnBlockComponent = newBlock.GetComponent<TurnBlock>();
        if (turnBlockComponent != null)
        {
            turnBlockComponent.Initialize(turn);
        }
        
        // 리스트에 삽입
        list.Insert(index, turnBlockComponent);

    }
    
    /// <summary>
    /// 맨 앞 블럭을 제거합니다.
    /// </summary>
    public void RemoveFrontBlock()
    {
        
        if (list.Count == 0)
        {
            Debug.LogWarning("Cannot remove front block: list is empty");
            return;
        }
        Debug.Log("RemovefrontBlock");
        // 첫 번째 블럭 제거
        var frontBlock = list[0];
        list.RemoveAt(0);
        
        // GameObject 제거
        if (frontBlock != null)
        {
            curTurnBlock?.Destroy();
            
            var tween = DOTween.Sequence();
            tween.Append(
                frontBlock.transform.DOLocalMove(frontBlock.transform.localPosition + curTurn.localPosition - nextTurns.localPosition, 1f)).
                Join(frontBlock.transform.DOScale(Vector3.one,1f)).
                AppendCallback(() => frontBlock.transform.SetParent(curTurn));
            curTurnBlock = frontBlock;
        }
        
        // 남은 블럭들을 앞으로 이동
        for (int i = 0; i < list.Count; i++)
        {
            var block = list[i].transform;
            block.DOLocalMoveY(-10 + i * -60, 1f);
        }
        var turn = GetFirstUncreatedTurn();
        if(turn != null)
        {
            InsertTurnBlock(list.Count, turn);
        }
    }
    
    /// <summary>
    /// 맨 뒤 블럭을 제거하고 uncreatedTurns 리스트에 추가합니다.
    /// </summary>
    public void RemoveBackBlock()
    {
        if (list.Count == 0)
        {
            Debug.LogWarning("Cannot remove back block: list is empty");
            return;
        }
        
        // 마지막 블럭 제거
        var lastBlock = list[list.Count - 1];
        
        // TurnBlock 컴포넌트에서 ITurn 정보를 가져와서 uncreatedTurns에 추가
        var turnBlockComponent = lastBlock.GetComponent<TurnBlock>();
        if (turnBlockComponent != null)
        {
            var turnData = turnBlockComponent.GetTurnData();
            if (turnData != null)
            {
                uncreatedTurns.Add(turnData);
            }
        }
        
        list.RemoveAt(list.Count - 1);
        
        // GameObject 제거
        if (lastBlock != null)
        {
            turnBlockComponent.Destroy();
        }
    }
    
    /// <summary>
    /// uncreatedTurns 리스트에서 턴을 가져옵니다.
    /// </summary>
    /// <returns>사용 가능한 턴이 있으면 true, 없으면 false</returns>
    public bool HasUncreatedTurns()
    {
        return uncreatedTurns.Count > 0;
    }
    
    /// <summary>
    /// uncreatedTurns 리스트의 첫 번째 턴을 가져옵니다.
    /// </summary>
    /// <returns>첫 번째 턴 또는 null</returns>
    public ITurn GetFirstUncreatedTurn()
    {
        if (uncreatedTurns.Count > 0)
        {
            var turn = uncreatedTurns[0];
            uncreatedTurns.RemoveAt(0);
            return turn;
        }
        return null;
    }
    private void OnEnable()
    {
        BattlePhase.OnTurnQueueChanged += OnTurnQueueChanged;
    }
    private void OnDisable()
    {
        BattlePhase.OnTurnQueueChanged -= OnTurnQueueChanged;
    }
}


