using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Augment
{
    [CreateAssetMenu(fileName = "ItemAugment", menuName = "Augment/Item Augment")]
    public class ItemAugment : AugmentSO
    {
        public List<ItemData> itemPool;
        public int countOnActive;
        public int countEveryLevel;

        public override void OnActive()
        {
            var factory = ItemFactory.Instance;
            if (factory == null) return;

            foreach(var newItemData in itemPool.GetRandomRange(countOnActive))
            {
                var newItem = factory.Request(newItemData);
                Agent.LocalPlayer.inventory.TryAddItem(newItem);
            }
        }

        public override void OnLevelStarted(StageData data)
        {
            var factory = ItemFactory.Instance;
            if (factory == null) return;

            foreach(var newItemData in itemPool.GetRandomRange(countEveryLevel))
            {
                var newItem = factory.Request(newItemData);
                Agent.LocalPlayer.inventory.TryAddItem(newItem);
            }
        }
    }
}