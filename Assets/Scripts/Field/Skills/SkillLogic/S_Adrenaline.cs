using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_Adrenaline : BaseSkillLogic
{
    public override UniTask<bool> InputSkill(IInput input)
    {
        return UniTask.FromResult(true);
    }

    public override IEnumerator ExecuteSkill()
    {
        var field = StageManager.Instance.field;
        
        // 광화 이펙트 재생
        EffectFactory.Instance.Request("PowUpAura", field.transform.position, field.transform.lossyScale*8);
        
        var entities = field.GetEntities();
        foreach (var entity in entities)
        {
            entity.Defense -= 1;
            entity.Power += 1;
        }
        // TODO : 광화 이펙트 대기

        yield return new WaitForSeconds(1f);
        
        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Adrenaline();
    }
}
