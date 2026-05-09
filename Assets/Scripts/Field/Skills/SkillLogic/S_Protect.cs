using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Protect : BaseSkillLogic
{
    private Entity _target;

    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            _target = entity;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _target = data[0];
            return true;
        }
        
        return false;

    }

    public override IEnumerator ExecuteSkill()
    {
        EffectFactory.Instance.Request("DefUpAura",_target.transform.position,_target.transform.lossyScale);
        _target.Defense += 2;
        
        _target = null;
        
        yield return new WaitForSeconds(1f);
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_Protect();
        return clone;
    }
}
