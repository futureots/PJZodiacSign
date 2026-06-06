using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "DebugAugment", menuName = "Augment/Debug Augment")]
    public class DebugAugment : AugmentSO
    {
        public override void OnActive()
        {
            EditorLogger.Print("Debug Augment.OnActive()");
        }

        public override void OnLevelStart(StageData data)
        {
            EditorLogger.Print("Debug Augment.OnLevelStart()");
        }

        public override void OnPhaseStart(Phase data)
        {
            EditorLogger.Print("Debug Augment.OnPhaseStart()");
        }

        public override void OnTurnStart(Turn data)
        {
            EditorLogger.Print("Debug Augment.OnTurnStart()");
        }
    }
}