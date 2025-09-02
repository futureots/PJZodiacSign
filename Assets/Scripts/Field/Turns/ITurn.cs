using System;

public interface ITurn
{
    /// <summary>
    /// 턴 시작 시 호출하는 함수
    /// </summary>
    /// <param name="onTurnEnd">턴 종료 시 호출할 콜백 함수</param>
    void StartTurn(Action onTurnEnd);

    int TeamNumber
    {
        get;
    }
}
