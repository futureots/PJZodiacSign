using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "Credit Augment", menuName = "Augment/Get Credit Augment")]
    public class GetCreditAugment : AugmentSO
    {
        public int CreditOnActive;
        public int CreditEveryLevel;

        public override void OnActive()
        {
            Agent.LocalPlayer.Credit += CreditOnActive;
        }

        public override void OnLevelStarted(StageData data)
        {
            Agent.LocalPlayer.Credit += CreditEveryLevel;
        }
    }
}