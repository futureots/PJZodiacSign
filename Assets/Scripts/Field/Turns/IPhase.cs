using System;

public interface IPhase
{
    public int Level {  get; }
    public PhaseType PhaseType { get; }
    /// <summary>
    /// 페이즈 시작
    /// </summary>
    /// <param name="onPhaseEnd">페이즈 종료 시 호출할 함수</param>
    void StartPhase(Action onPhaseEnd);
}

