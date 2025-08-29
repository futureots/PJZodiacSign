using System;

public interface ITurn
{
    void StartTurn(Action onTurnEnd);

    Agent Agent
    {
        get;
    }
}
