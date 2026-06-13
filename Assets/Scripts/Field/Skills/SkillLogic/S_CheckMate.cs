using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_CheckMate : BaseSkillLogic
{
    [SerializeField] int modifier;
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

    public override bool IsValuable()
    {
        if (component.TryGetComponent<Entity>(out var owner))
        {
            _owner = owner;
            var tiles = _owner.GetAttackArea();
            
            // 적이 1명이상 있으면 사용
            return tiles.Exists(tile => !tile.IsEmpty && !tile.occupiedEntity.team.IsAlly(_owner.team));
        }

        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetAttackArea();

        EffectFactory.Instance.Request("ManaAura", _owner.transform.position, _owner.transform.lossyScale * 3);
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (!tile.occupiedEntity.team.IsAlly(_owner.team))
            {
                Entity entity = tile.occupiedEntity;
                component.StartCoroutine(Hit(entity,_owner.energy.CurEnergy * modifier));
            }
        }

        yield return new WaitForSeconds(1.5f);
        _owner = null;
        yield break;
    }
    
    IEnumerator Hit(Entity entity, int damage)
    {
        EffectFactory.Instance.Request("LightningStrike",entity.transform.position,entity.transform.lossyScale);
        yield return new WaitForSeconds(0.5f);
        entity.Damaged(damage);
    }
    
    public override BaseSkillLogic Clone()
    {
        var clone = new S_CheckMate();
        clone.modifier = modifier;
        return clone;
    }
}
