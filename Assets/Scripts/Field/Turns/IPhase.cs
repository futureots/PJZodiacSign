using System;

public interface IPhase
{
    public int Level {  get; }
    void StartPhase(Action onPhaseEnd);
}

