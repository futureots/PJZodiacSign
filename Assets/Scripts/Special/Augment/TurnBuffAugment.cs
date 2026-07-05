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
        public int applyValueOnEveryPhase;
        public int applyValueOnEveryTurn;
        public TargetStat targetStat;

        public readonly List<Entity> affectedEntity = new();

        public override void OnPhaseStarted(Phase data)
        {
            if (data.phaseName != PhaseType.Battle) return;
            var agent = Agent.LocalPlayer;
            
            // 해당 entity 선택
            bool applyAllEntity = count <= 0;

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
                affectedEntity.Add(entity);
            }

            ApplyBuff(applyValueOnEveryPhase);
        }

        public override void OnTurnStarted(Turn data, uint turnCount)
        {
            if (data.agentID != Agent.LocalPlayer.id) return;
            if (data.type != TurnType.ACTION) return;
            var agent = Agent.LocalPlayer;
            
            // 해당 entity 선택
            bool applyAllEntity = count <= 0;

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
                affectedEntity.Add(entity);
            }

            ApplyBuff(applyValueOnEveryTurn);
        }

        /// <summary>
        /// 스탯에 따른 버프 적용
        /// </summary>
        /// <param name="targetStat">대상 스탯</param>
        /// <param name="applyValue">적용 값</param>
        public void ApplyBuff(int applyValue)
        {
            foreach(var entity in affectedEntity)
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
                        break;
                    case TargetStat.MP:
                        entity.energy.CurEnergy += applyValue;
                        break;
                    case TargetStat.ATK:
                        entity.Power += applyValue;
                        break;
                    case TargetStat.Lvl:
                        entity.Level += applyValue;
                        break;
                    default:
                        return;
                }
            }
        }
    }
}