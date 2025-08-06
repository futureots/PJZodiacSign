using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PowerModifier", menuName = "Scriptable Objects/Buff/PowerModifier")]
public class PowerModifier : BuffData
{
    public override void ApplyBuff(Entity entity, int count)
    {
        entity.Power += count;
    }

    public override void ExtendBuff(Entity entity, ref int currentCount, int count)
    {
        entity.Power += count;
        currentCount += count;
    }

    public override void RemoveBuff(Entity entity, int count)
    {
        entity.Power -= count;
    }

    public override void UpdateBuff(Entity entity, ref int count)
    {
        
    }
}
