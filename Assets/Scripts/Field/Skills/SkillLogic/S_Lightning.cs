using Cysharp.Threading.Tasks;
using System;
using System.Collections;

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
        EffectFactory.Instance.RequestEffect("ManaAura",field.transform.position,field.transform.lossyScale);
        
        var entities = field.GetEntities();
        foreach (var entity in entities)
        {
            entity.Damaged(entity.MaxHealth/4);
        }
        // TODO : 낙뢰 이펙트 시간 기다리기

        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Lightning();
    }
}
