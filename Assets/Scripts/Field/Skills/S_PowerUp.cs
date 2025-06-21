using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class S_PowerUp : BaseSkill<SD_PowerUp>
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;

    public S_PowerUp(SD_PowerUp data) : base(data)  {}

    public override void Activate()
    {
        target.AddBuff(new PowerModifier(), 3);
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
