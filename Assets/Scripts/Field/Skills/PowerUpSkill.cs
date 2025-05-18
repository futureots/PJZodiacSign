using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PowerUpSkill : Skill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;
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
