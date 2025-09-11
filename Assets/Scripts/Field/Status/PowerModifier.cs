using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PowerModifier", menuName = "Scriptable Objects/Buff/PowerModifier")]
public class PowerModifier : BuffData
{
    Entity entity;
    public override void ApplyBuff(GameObject target, int count)
    {
        entity = target.GetComponent<Entity>();
        entity.Power += count;
    }

    public override void ExtendBuff(GameObject target, ref int currentCount, int count)
    {
        entity.Power += count;
        currentCount += count;
    }

    public override void RemoveBuff(GameObject target, int count)
    {
        entity.Power -= count;
    }

    public override void UpdateBuff(GameObject target, ref int count)
    {
        
    }
}
