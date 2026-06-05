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
        public string effectName = "";
        public string description = "";
        [SerializeField] private AugmentType _type = AugmentType.Neutral;
        public AugmentType Type => _type;
        
        public virtual void OnActive() { }

        public virtual void OnLevelStart() { }

        public virtual void OnPhaseStart() { }

        public virtual void OnTurnStart() { }
    }
}
