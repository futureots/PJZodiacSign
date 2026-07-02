using System;
using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "ActCount", menuName = "Augment/Act Count Augment")]
    public class ActCount : AugmentSO
    {
        public int maxCount;

        public override void OnActive()
        {
            Agent.LocalPlayer.actionCount = Math.Min(Agent.LocalPlayer.actionCount + 1, maxCount);
        }

        public override void OnLevelStarted(StageData data)
        {
            Agent.LocalPlayer.actionCount = Math.Min(Agent.LocalPlayer.actionCount + 1, maxCount);
        }
    }
}