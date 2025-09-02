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
        base.Execute();
    }
    public override string ToString()
    {
        if( skill is AbstractSkillInstance sk)
        {
            if(skill is S_BaseEntity<BaseSkillData> s)
            {
                return $"{s.Owner.name}이 {s.skillName} 사용";
            }
            return $"{sk.skillName} 사용";
        }
        return "";
            
    }
}
