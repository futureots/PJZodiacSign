using UnityEngine;


[CreateAssetMenu(fileName = "PowerModifier", menuName = "Scriptable Objects/Buff/PowerModifier")]
public class PowerModifier : BuffData
{
    PowerComponent power;
    public override bool ApplyBuff(GameObject target, int count)
    {
        if (target.TryGetComponent<PowerComponent>(out var component))
        {
            power = component;
            power.Power += count;
            return true;
        }
        else return false;
    }

    public override void ExtendBuff(GameObject target, ref int currentCount, int count)
    {
        power.Power += count;
        currentCount += count;
    }

    public override void RemoveBuff(GameObject target, int count)
    {
        power.Power -= count;
    }

    public override void UpdateBuff(GameObject target, ref int count)
    {
        
    }
}
