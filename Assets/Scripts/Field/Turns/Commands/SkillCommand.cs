using System;
using System.Collections;

public class SkillCommand : Command
{

    public readonly SkillComponent _skill;
    
    public SkillCommand(SkillComponent skill)
    {
        this._skill = skill;
    }

    public override IEnumerator Execute(Action callback)
    {
        EditorLogger.Print("SkillCommandExecute");
        yield return _skill.StartCoroutine(_skill.ExecuteSkill());
        callback?.Invoke();
        Delete();
        yield break;
    }
    public override string ToString()
    {
        string text = "";
        text += $"스킬사용";
        return text;
            
    }
}
