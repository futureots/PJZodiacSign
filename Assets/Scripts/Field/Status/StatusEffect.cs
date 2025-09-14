using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/Buff/StatusEffect")]
public class StatusEffect : BuffData
{
    public override bool ApplyBuff(GameObject target, int count)
    {
        // 해당 오브젝트가 버프 부여가 가능한 경우 true 반환
        var entity = target.GetComponent<Entity>();
        entity.AddEffect(id);
        return true;
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
