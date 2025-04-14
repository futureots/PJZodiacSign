using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : Command
{
    //입력값이 모두 입력된 스킬
    public ISkill skill;

    public SkillCommand(ISkill skill)
    {
        this.skill = skill;
    }
    public override void Execute()
    {
        skill.ExecuteSkillSequence();
    }
}
