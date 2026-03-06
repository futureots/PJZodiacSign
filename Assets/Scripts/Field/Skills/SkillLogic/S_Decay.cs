using Cysharp.Threading.Tasks;
using System;
using System.Collections;


[Serializable]
public class S_Decay : BaseSkillLogic
{
    private Entity _target;
    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var target))
        {
            _target = target;
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
        _target.Defense -= 2;
        
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Decay();
    }
}
