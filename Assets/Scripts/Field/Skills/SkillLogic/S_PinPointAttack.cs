using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 정밀사격 스킬 로직
/// </summary>
[Serializable]
public class S_PinPointAttack : BaseSkillLogic
{
    [SerializeField] private BasicAttackEffect attackEffect;
    private Entity _owner;
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
        
        var opponentList = StageManager.Instance.field.GetEntities(entity.team.teamNumber,false);
        opponentList.Sort((a, b) => a.CurHealth.CompareTo(b.CurHealth));
        var data2 = await input.InputEntity(opponentList, 1);
        if (data2 == null) 
        {
            return false;
        }
        _owner =  entity;
        _target = data2[0];
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        var damage = _owner.Power;


        var target = _target;

        var effect = EffectFactory.Instance.RequestEffect("AttackEffect",_owner.transform.position + Vector3.up * 7,_owner.transform.lossyScale,5f);
        if (effect.TryGetComponent(out BasicAttackEffect atkObj))
        {
            atkObj.Initialize(target.gameObject, Hit);
        }
        
        yield return new WaitForSeconds(1.5f);
        
        _owner = null;
        _target = null;
        
        yield break;
        
        void Hit()
        {
            target.Damaged(damage);
            target.Defense -= 1;
        }
    }

    
    public override BaseSkillLogic Clone()
    {
        var clone =  new S_PinPointAttack();
        clone.Init(attackEffect);
        return clone;
    }
}
