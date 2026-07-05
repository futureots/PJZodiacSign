using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Augment
{
    [CreateAssetMenu(fileName = "SellEntityAugment", menuName = "Augment/Sell and Get Credit Augment")]
    public class SellEntityAugment : AugmentSO
    {
        public int sellEntityCount;
        public int creditMultiplier = 1;

        private bool applyAll => sellEntityCount <= 0;

        public override void OnActive()
        {
            int totalSellCredit = 0;
            List<Entity> entityPool = new (Agent.LocalPlayer.resourceEntities);
            entityPool.AddRange(Agent.LocalPlayer.fieldEntities);

            var sellPool = applyAll switch
            {
                true => entityPool,
                false => entityPool.GetRandomRange(sellEntityCount)
            };

            // 판매 및 제거
            foreach(var entity in sellPool)
            {
                totalSellCredit += entity.baseData.normalPrice;
                entity.Dead();
            }

            // 금액 추가
            Agent.LocalPlayer.Credit += totalSellCredit;

            // 금액 배율
            Agent.LocalPlayer.Credit *= creditMultiplier;
        }
    }
}