using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DebugSkillLogic : BaseSkillLogic
{
    public override IEnumerator ExecuteSkill()
    {
        Debug.Log("SkillStart");
        yield return new WaitForSeconds(1);
        Debug.Log("SkillEnd");
    }

    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new DebugSkillLogic();
    }
}
