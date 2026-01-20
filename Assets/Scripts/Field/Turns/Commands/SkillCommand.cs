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
        text += $"{Skill.skillData.skillName} 스킬 사용";
        return text;
    }
    public override bool IsOverlap(Command cmd)
    {
        if (cmd is SkillCommand skCmd)
        {
            if (skCmd.Skill == Skill)
            {
                return true;
            }
        }
        return false;
    }
}
