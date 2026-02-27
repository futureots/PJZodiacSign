using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_PinPointAttack : BaseSkillLogic
{
    [SerializeField] private BasicAttackEffect attackEffect;
    private Entity _entity;
    private Entity _target;

    public void Init(BasicAttackEffect effect)
    {
        attackEffect =  effect;
    }

    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        Entity entity = null;
        if(component.TryGetComponent<Entity>(out var owner))
        {
            entity = owner;
        }
        else
        {
            var data = await input.InputEntity(list, 1);
            if(data != null)
            {
                entity = data[0];
            }
            else
            {
                return false;
            }
        }
        list.Remove(entity);
        
        var data2 = await input.InputEntity(list, 1);
        if (data2 == null) 
        {
            return false;
        }
        _entity =  entity;
        _target = data2[0];
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        var damage = _entity.Power;
        
        var effect = GameObject.Instantiate(attackEffect, _entity.transform.position + Vector3.up * 7, Utils.QI);
        effect?.Initialize(_target.gameObject, damage);
        
        yield return new WaitForSeconds(1.5f);
        
        _entity = null;
        _target = null;
        
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone =  new S_PinPointAttack();
        clone.Init(attackEffect);
        return clone;
    }
}
