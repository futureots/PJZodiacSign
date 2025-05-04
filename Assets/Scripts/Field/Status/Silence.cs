using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : BuffData
{
    public override void ApplyBuff(Entity entity, int count)
    {
        Debug.Log("Buff Attach Slience");
        entity.slienceCount++;

    }

    public override void RemoveBuff(Entity entity,int count)
    {
        entity.slienceCount = Mathf.Max(0, entity.slienceCount - 1);
        Debug.Log("Buff Detach Slience");
    }

    public override void UpdateBuff(Entity entity,ref int count)
    {
        count--;
        Debug.Log("Buff Update Slience" + count);
    }
}
