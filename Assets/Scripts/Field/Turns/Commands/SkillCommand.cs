using System;
using System.Collections;


public sealed record SkillCommand(SkillComponent Skill) : Command
{
    public override IEnumerator Execute(Action callback)
    {
        yield return Skill.StartCoroutine(Skill.ExecuteSkill());
        callback?.Invoke();
        Delete();
    }
    public override string ToString()
    {
        string text = "";
        text += $"스킬사용";
        return text;
    }
}
