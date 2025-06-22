using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class S_SingleBuff : BaseSkillInstance<SD_SingleBuff>
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;

    public S_SingleBuff() : base() { }
    public S_SingleBuff(SD_SingleBuff data) : base(data) { }

    public override void Activate()
    {
        Debug.Log(data);
        target.AddBuff(data.buffData, data.count);
    }

    public override bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public override void Reinitialize()
    {
        target = null;
    }

}
