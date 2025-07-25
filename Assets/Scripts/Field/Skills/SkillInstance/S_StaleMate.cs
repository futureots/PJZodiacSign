using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

using static UnityEngine.EventSystems.EventTrigger;

public class S_StaleMate : S_BaseEntity<SD_StaleMate>
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;

    public S_StaleMate(SD_StaleMate data, Entity owner = null) : base(data, owner)
    {
    }

    public override void Activate()
    {
        Debug.Log(data);
        target.AddBuff(new Root(), 1);
    }

    public override bool IsValidInput(FieldInfo field)
    {
        if(field.Name == nameof(target))
        {
            return IsValidEntity();
        }
        return false;
    }
    bool IsValidEntity()
    {
        if (target == null) return false;
        if (target.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    public override void Reinitialize()
    {
        target = null;
    }

}
