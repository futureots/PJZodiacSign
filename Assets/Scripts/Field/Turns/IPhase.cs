using System;

public interface IPhase
{
    void StartPhase(Action onPhaseEnd);
}

