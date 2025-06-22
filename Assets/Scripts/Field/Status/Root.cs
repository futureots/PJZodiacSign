using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Root", menuName = "Scriptable Objects/Buff/Root")]
public class Root : BuffData
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
    public override void RemoveBuff(Entity entity, int count)
    { 
    }

    public override void UpdateBuff(Entity entity, ref int count)
    {
        count = Mathf.Max(count - 1, 0);
        Debug.Log("Buff Update Root" + count);
    }
}
