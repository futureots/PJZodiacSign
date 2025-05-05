using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerModifier : BuffData
{
    public override void ApplyBuff(Entity entity, int count)
    {
        entity.power += count;
    }

    public override void ExtendBuff(Entity entity, ref int currentCount, int count)
    {
        entity.power += count;
        currentCount += count;
    }

    public override void RemoveBuff(Entity entity, int count)
    {
        entity.power -= count;
    }

    public override void UpdateBuff(Entity entity, ref int count)
    {
        
    }
}
