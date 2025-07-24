using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class S_BaseEntity : BaseSkillInstance
{    
    public S_BaseEntity(BaseSkillData data, Entity owner = null) : base(data)
    {
        Owner = owner;
    }

    // 스킬 시전자 엔티티, 없으면 시전자도 선택가능
    [SkillTarget("스킬 시전자를 선택해주세요.")]
    public Entity Owner;

}
