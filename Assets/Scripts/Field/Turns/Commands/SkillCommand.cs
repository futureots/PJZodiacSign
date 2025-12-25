using Condition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : Command
{
    #region oldSkillCommand
    //입력값이 모두 입력된 스킬
    public IActive skill;

    public SkillCommand(IActive skill)
    {
        selecterObjects = new();
        this.skill = skill;
    }

    #endregion

    #region newCommand

    public SkillComponent _skill;
    public List<ConditionArgs> args;
    
    public SkillCommand(SkillComponent skill, List<ConditionArgs> args)
    {
        this._skill = skill;
        this.args = args;
    }

    #endregion

    public override void Execute()
    {
        //var result = skill.ExecuteSequence();
        var result = _skill.ExecuteSkill(args.ToArray());
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
