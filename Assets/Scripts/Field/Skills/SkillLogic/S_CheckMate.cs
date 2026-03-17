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
    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetAttackArea();

        EffectFactory.Instance.RequestEffect("ManaAura", _owner.transform.position, _owner.transform.lossyScale * 3);
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (!entity.team.IsAlly(_owner.team))
                {
                    component.StartCoroutine(Hit(entity,_owner.energy.CurEnergy * modifier));
                }
                
            }
        }

        yield return new WaitForSeconds(1.5f);
        _owner = null;
        yield break;
    }
    
    IEnumerator Hit(Entity entity, int damage)
    {
        EffectFactory.Instance.RequestEffect("LightningStrike",entity.transform.position,entity.transform.lossyScale);
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
