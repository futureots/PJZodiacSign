using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : BuffData
{
    public override void ApplyBuff(Entity entity, int count)
    {

    }
    public override void ExtendBuff(Entity entity, ref int currentCount, int count)
    {
        if (currentCount < count)
        {
            currentCount = count;
        }
    }
    public override void RemoveBuff(Entity entity,int count)
    {
    }

    public override void UpdateBuff(Entity entity,ref int count)
    {
        count = Mathf.Max(count - 1, 0);
    }
}
