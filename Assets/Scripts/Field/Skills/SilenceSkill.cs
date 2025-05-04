using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SilenceSkill : MonoBehaviour,ISkill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;
    public void Activate()
    {
        target.AddBuff(new Silence(), 3);
    }

    public bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public void Reinitialize()
    {
        target = null;
    }

}
