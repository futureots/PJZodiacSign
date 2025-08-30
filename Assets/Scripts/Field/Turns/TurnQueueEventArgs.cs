using System;

public class TurnQueueEventArgs : EventArgs
{
    public ITurn Turn { get; set; }
    public TurnQueueEventType EventType { get; set; }
    public int Index { get; set; }
    
    public TurnQueueEventArgs(ITurn turn, TurnQueueEventType eventType, int index = -1)
    {
        Turn = turn;
        EventType = eventType;
        Index = index;
    }
}

public enum TurnQueueEventType
{
    TurnAdded,      // 턴이 뒤에 추가됨
    TurnInserted,   // 턴이 특정 위치에 삽입됨
    TurnRemoved,    // 턴이 제거됨
    TurnMoved,      // 턴이 이동됨
    TurnStarted     // 턴이 시작됨
}

