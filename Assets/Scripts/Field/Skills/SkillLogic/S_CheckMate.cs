using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_CheckMate : BaseSkillLogic
{
    [SerializeField] int damage;
    private Entity _owner;

    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var owner))
        {
            _owner = owner;
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
        var tiles = _owner.GetAttackArea();
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (!entity.team.IsAlly(_owner.team))
                {
                    entity.Damaged(_owner.energy.CurEnergy * damage);
                }
                
            }
        }
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_CheckMate();
        clone.damage = damage;
        return clone;
    }
}
