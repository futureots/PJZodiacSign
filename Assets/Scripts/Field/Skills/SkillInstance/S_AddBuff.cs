using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using static UnityEngine.EventSystems.EventTrigger;

public class S_AddBuff : S_BaseEntity<SD_AddBuff>
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity target;


    public S_AddBuff(SD_AddBuff data, Entity owner = null) : base(data, owner)
    {
    }

    public override void Activate()
    {
        Debug.Log(data);
        target.AddBuff(data.BuffData, data.count);
    }

    public override bool IsValidInput(FieldInfo field)
    {
        if (field.Name == nameof(target))
        {
            return IsValidEntity(target);
        }
        else
        {
            return field.GetValue(this) != null;
        }
    }
    bool IsValidEntity(Entity other)
    {
        if (other == null) return false;
        //공격 범위 내 적만 속박
        if (Owner.GetAttackArea().Contains(other.curTile))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 스킬 사용에 필요한 값을 알아서 가져옴.
    /// </summary>
    /// <param name="field">현재 스킬을 사용한 필드</param>
    /// <returns>스킬 사용이 가능한지 true, 아니면 false</returns>
    public override bool CanSkillInput(Field field)
    {
        var other = GetValidEntities(field);
        if (other.Count > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 스킬 입력이 가능한 경우에만 스킬의 입력값을 할당함.
    /// </summary>
    /// <param name="field">스킬을 사용할 필드</param>
    public override bool SetSkillInput(Field field)
    {
        var entities = GetValidEntities(field);
        if(entities.Count > 0)
        {
            target = entities[UnityEngine.Random.Range(0, entities.Count)];
            return true;
        }
        return false;
    }
    List<Entity> GetValidEntities(Field field)
    {
        var list = field.GetOccupiedObjects();
        var other = new List<Entity>();
        foreach (var item in list)
        {
            var entity = item.GetComponent<Entity>();
            if (entity != null)
            {
                if (IsValidEntity(entity))
                {
                    other.Add(entity);
                }
            }
        }
        return other;
    }

}
