using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 정밀사격 스킬 로직
/// </summary>
[Serializable]
public class S_PinPointAttack : BaseSkillLogic
{
    private Entity _owner;
    private List<Entity> _targetList;

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
        _targetList = data2;
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        var damage = _owner.Power;
        var targetList = _targetList;

        foreach (var entity in targetList)
        {
            var curTarget = entity;
            var effect = EffectFactory.Instance.Request("AttackEffect",_owner.transform.position + Vector3.up * 7,_owner.transform.lossyScale,5f);
            if (effect.TryGetComponent(out BasicAttackEffect atkObj))
            {
                atkObj.Initialize(curTarget.gameObject, () => Hit(curTarget));
            }
        }
        
        yield return new WaitForSeconds(1.5f);
        
        _owner = null;
        _targetList.Clear();
        
        yield break;
        
        void Hit(Entity target)
        {
            target.Damaged(damage);
            target.Defense -= 1;
        }
    }

    
    public override BaseSkillLogic Clone()
    {
        var clone =  new S_PinPointAttack();
        return clone;
    }
}
