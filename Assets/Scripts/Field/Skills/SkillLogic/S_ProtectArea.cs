using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_ProtectArea : BaseSkillLogic
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

    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetMoveArea();
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (entity.team.IsAlly(_owner.team))
                {
                    entity.Defense += 1;
                }
                
            }
        }
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_ProtectArea();
        return clone;
    }
}
