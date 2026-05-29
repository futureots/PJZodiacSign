using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_PowerDown : BaseSkillLogic
{
    private Entity _owner;
    
    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            _owner = entity;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _owner = data[0];
            return true;
        }
        
        return false;

    }

    public override bool IsValuable()
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            var tiles = entity.GetAttackArea();
            
            // 적이 1명이라도 있으면 사용
            return tiles.Exists(tile => !tile.IsEmpty && !tile.occupiedEntity.team.IsAlly(_owner.team));
        }

        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetAttackArea();
        
        EffectFactory.Instance.Request("PowDownAura",_owner.transform.position,_owner.transform.lossyScale*3);
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (!entity.team.IsAlly(_owner.team))
                {
                    entity.Power -= 1;
                }
            }
        }
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_PowerDown();
        return clone;
    }
}
