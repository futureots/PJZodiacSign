using UnityEngine;

namespace Augment
{
    public abstract class AugmentSO : ScriptableObject, IAugmentEffect
    {
        public string effectName = "";
        public string description = "";
        
        public virtual void OnActive() { }

        public virtual void OnLevelStart() { }

        public virtual void OnPhaseStart() { }

        public virtual void OnTurnStart() { }
    }
}
