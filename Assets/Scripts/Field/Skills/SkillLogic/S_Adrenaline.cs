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
        //TODO : 광화 이펙트 재생
        var field = StageManager.Instance.field;
        var effect = EffectFactory.Instance.RequestEffect("Aura", field.transform.position, Vector3.Scale(field.transform.lossyScale,new Vector3(8,1,8)));
        var entities = field.GetEntities();
        foreach (var entity in entities)
        {
            entity.Defense -= 1;
            entity.Power += 1;
        }
        // TODO : 광화 이펙트 대기

        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Adrenaline();
    }
}
