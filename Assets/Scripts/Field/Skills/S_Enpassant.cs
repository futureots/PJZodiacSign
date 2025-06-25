using System.Reflection;
using UnityEngine;

public class S_Enpassant : S_BaseEntity
{
    public S_Enpassant(BaseSkillData data, Entity owner = null) : base(data, owner)
    {
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsActable()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsValidInput(FieldInfo field)
    {
        throw new System.NotImplementedException();
    }

    public override void Reinitialize()
    {
        throw new System.NotImplementedException();
    }
}
