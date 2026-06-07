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

        public override void OnLevelStarted(StageData data)
        {
            EditorLogger.Print("Debug Augment.OnLevelStarted()");
        }

        public override void OnPhaseStarted(Phase data)
        {
            EditorLogger.Print("Debug Augment.OnPhaseStarted()");
        }

        public override void OnTurnStarted(Turn data, uint turnCount)
        {
            EditorLogger.Print("Debug Augment.OnTurnStarted()");
        }
    }
}