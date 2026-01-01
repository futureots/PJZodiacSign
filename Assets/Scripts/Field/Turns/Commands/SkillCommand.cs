using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : Command
{

    public SkillComponent _skill;
    
    public SkillCommand(SkillComponent skill)
    {
        this._skill = skill;
    }

    public override IEnumerator Execute(Action callback)
    {
        //var result = skill.ExecuteSequence();
        yield return _skill.StartCoroutine(_skill.ExecuteSkill());
        callback?.Invoke();
        
    }
    public override string ToString()
    {
        string text = "";
        text += $"{_skill.skillData.skillName} 사용";
        return text;
            
    }
}
