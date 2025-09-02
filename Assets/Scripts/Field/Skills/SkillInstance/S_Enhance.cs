using System.Reflection;
using UnityEngine;

public class S_Enhance : BaseSkillInstance<SD_Enhance>
{
    [SkillTarget("강화 대상을 선택하세요.")]
    public Entity entity;
    public S_Enhance(SD_Enhance data) : base(data)
    {
    }

    public override void Activate()
    {
        entity.Level += 1;
    }

    public override bool CanSkillInput(Field field)
    {
        return true;
    }

    public override bool IsValidInput(FieldInfo field)
    {
        return entity !=null;
    }


    public override bool SetSkillInput(Field field)
    {
        return true;
    }
}
