using System;
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
    
    public SkillCommand(SkillComponent skill)
    {
        this._skill = skill;
    }

    #endregion

    public override IEnumerator Execute(Action callback)
    {
        //var result = skill.ExecuteSequence();
        yield return _skill.StartCoroutine(_skill.ExecuteSkill());
        callback?.Invoke();
        
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
