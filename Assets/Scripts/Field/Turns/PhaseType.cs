using System;

namespace Battle.Phase
{
    [Flags]
    public enum PhaseType
    {
        None = 0,
        Battle = 1 << 0,
        Repair = 1 << 1
    }

    public interface IPhaseManageService
    {
        public PhaseType CurPhase { get; }
    }
}