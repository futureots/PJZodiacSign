using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/Buff/StatusEffect")]
public class StatusEffect : BuffData
{
    public override void ApplyBuff(GameObject target, int count)
    {
        var entity = target.GetComponent<Entity>();
        entity.AddEffect(id);
    }
    public override void ExtendBuff(GameObject target, ref int currentCount, int count)
    {
        if (currentCount < count)
        {
            currentCount = count;
        }
    }
    public override void RemoveBuff(GameObject target, int count)
    {
        var entity = target.GetComponent<Entity>();
        entity.SubtractEffect(id);
    }

    public override void UpdateBuff(GameObject target, ref int count)
    {
        count = Mathf.Max(count - 1, 0);
        Debug.Log("Buff Update Root" + count);
    }
}
