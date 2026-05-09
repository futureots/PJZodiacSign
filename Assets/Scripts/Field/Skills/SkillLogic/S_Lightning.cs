using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Lightning : BaseSkillLogic
{
    
    public override UniTask<bool> InputSkill(IInput input)
    {
        return UniTask.FromResult(true);
    }

    public override IEnumerator ExecuteSkill()
    {
        //TODO : 낙뢰 이펙트 재생
        var field = StageManager.Instance.field;
        EffectFactory.Instance.Request("ManaAura",field.transform.position,Vector3.Scale(field.transform.lossyScale,new Vector3(8,1,8)));
        
        var entities = field.GetEntities();
        foreach (var entity in entities)
        {
            component.StartCoroutine(LightningHit(entity));
        }

        yield return new WaitForSeconds(1.5f);

        yield break;
    }

    IEnumerator LightningHit(Entity entity)
    {
        EffectFactory.Instance.Request("LightningStrike",entity.transform.position,entity.transform.lossyScale);
        yield return new WaitForSeconds(0.5f);
        entity.Damaged(entity.MaxHealth/4);
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Lightning();
    }
}
