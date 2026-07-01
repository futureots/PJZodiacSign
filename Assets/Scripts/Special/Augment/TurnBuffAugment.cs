using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Augment
{
    public enum TargetStat
    {
        HP,
        MP,
        ATK,
        Lvl,
    }
    
    [CreateAssetMenu(fileName = "BuffAugment", menuName = "Augment/Add Stat")]
    public class TurnBuffAugment : AugmentSO
    {
        public int count;
        public int applyValue;
        public TargetStat targetStat;

        private readonly List<Entity> affectedEntity = new();

        public override void OnTurnStarted(Turn data, uint turnCount)
        {
            if (data.agentID != Agent.LocalPlayer.id) return;
            if (data.type != TurnType.ACTION) return;
            var agent = Agent.LocalPlayer;
            
            // 해당 entity 선택
            bool applyAllEntity = count == 0;

            List<Entity> targetEntity;
            if (applyAllEntity)
            {
                targetEntity = agent.fieldEntities;
            }
            else
            {
                targetEntity = agent.fieldEntities.GetRandomRange(count);
            }

            affectedEntity.Clear();
            foreach (var entity in targetEntity)
            {
                ApplyBuff(entity, targetStat, applyValue);
                affectedEntity.Add(entity);
            }
        }

        /// <summary>
        /// 스탯에 따른 버프 적용
        /// </summary>
        /// <param name="entity">대상 엔티티</param>
        /// <param name="targetStat">대상 스탯</param>
        /// <param name="applyValue">적용 값</param>
        private void ApplyBuff(Entity entity, TargetStat targetStat, int applyValue)
        {
            switch (targetStat)
            {
                // 공격
                case TargetStat.HP:
                    if (applyValue > 0)
                    {
                        entity.Healed(applyValue);
                    }
                    else
                    {
                        entity.Damaged(applyValue);
                    }
                    return;
                case TargetStat.MP:
                    var energy = entity.energy;
                    energy.CurEnergy += applyValue;
                    return;
                case TargetStat.ATK:
                    entity.Power += applyValue;
                    return;
                case TargetStat.Lvl:
                    entity.Level += applyValue;
                    return;
                default:
                    return;
            }
        }
    }
}