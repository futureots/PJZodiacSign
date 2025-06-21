using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : Command
{
    //입력값이 모두 입력된 스킬
    public IActive skill;

    public SkillCommand(IActive skill)
    {
        selecterObjects = new();
        this.skill = skill;
    }
    public override void Execute()
    {
        var result = skill.ExecuteSequence();
        //Debug.Log(boolean);
    }
    public override void Delete()
    {
        base.Delete();
        skill.Reinitialize();
    }
}
