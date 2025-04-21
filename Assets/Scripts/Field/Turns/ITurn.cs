using System;

public interface ITurn
{
    void Execute(Action onTurnEnd);
}
