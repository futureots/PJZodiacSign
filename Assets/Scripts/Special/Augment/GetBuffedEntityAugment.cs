using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "GetBuffedEntity", menuName = "Augment/Get Buffed Entity")]
    public class GetBuffedEntityAugment: AugmentSO
    {
        [SerializeField] private GetEntityAugment getEntityAugment;
        [SerializeField] private TurnBuffAugment buffAugment;

        public override void OnActive()
        {
            getEntityAugment.OnActive();
            foreach(var entity in getEntityAugment.addedEntity)
            {
                buffAugment.affectedEntity.Add(entity);
            }
        }

        public override void OnTurnStarted(Turn data, uint turnCount)
        {
            buffAugment.ApplyBuff(buffAugment.applyValueOnEveryTurn);
        }
    }
}