using System;
using UnityEngine;

namespace Augment
{
    [Flags]
    public enum AugmentType
    {
        Add = 0,
        Neutral = 1 << 0,
        Positive = 1 << 1,
        Negative = 1 << 2,
    }
    
    public abstract class AugmentSO : ScriptableObject, IAugmentEffect
    {
        // Augment Data
        public string effectName = "";
        public string description = "";
        [SerializeField] private AugmentType type = AugmentType.Neutral;
        public AugmentType Type => type;
        
        public virtual void OnActive() { }

        public virtual void OnLevelStart(StageData data) { }

        public virtual void OnPhaseStart(Phase data) { }

        public virtual void OnTurnStart(Turn data) { }
    }
}
