using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;


[Serializable]
public class S_FullHeal : BaseSkillLogic
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
        
        
        EffectFactory.Instance.RequestEffect("HealAura", _target.transform.position, _target.transform.lossyScale);
        yield return new WaitForSeconds(0.1f);
        _target.CurHealth = _target.MaxHealth;
        yield return new WaitForSeconds(0.9f);
        
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_FullHeal();
    }
}
