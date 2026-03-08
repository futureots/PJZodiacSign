using Cysharp.Threading.Tasks;
using System;
using System.Collections;


[Serializable]
public class S_Adrenaline : BaseSkillLogic
{
    public override UniTask<bool> InputSkill(IInput input)
    {
        return UniTask.FromResult(true);
    }

    public override IEnumerator ExecuteSkill()
    {
        //TODO : 지진 이펙트 재생
        var entities = StageManager.Instance.field.GetEntities();
        foreach (var entity in entities)
        {
            entity.Defense -= 1;
            entity.Power += 1;
        }
        // TODO : 지진 이펙트 시간 기다리기

        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Adrenaline();
    }
}
