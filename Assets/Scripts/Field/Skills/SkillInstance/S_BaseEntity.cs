using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class S_BaseEntity<T> : BaseSkillInstance<T>,IOwnable where T : BaseSkillData
{    
    public S_BaseEntity(T data, Entity owner = null) : base(data)
    {
        _owner = owner;
    }

    // 스킬 시전자 엔티티, 없으면 시전자도 선택가능
    [SkillTarget("스킬 시전자를 선택해주세요.")]
    public Entity _owner;

    public Entity Owner { 
        get
        {
            return _owner;
        }
        set {
            _owner = value; 
        }
    }
}
public interface IOwnable
{
    public Entity Owner { get; set; }
}