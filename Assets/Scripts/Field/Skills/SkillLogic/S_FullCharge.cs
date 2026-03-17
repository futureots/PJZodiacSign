using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_FullCharge : BaseSkillLogic
{
    private Entity _target;
    public override async UniTask<bool> InputSkill(IInput input)
    {
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
        
        // Effect
        EffectFactory.Instance.RequestEffect("ManaAura",_target.transform.position,_target.transform.lossyScale);
        yield return new WaitForSeconds(0.1f);
        
        _target.energy.CurEnergy = _target.energy.MaxEnergy;
        yield return new WaitForSeconds(0.9f);
        
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_FullCharge();
    }
}
