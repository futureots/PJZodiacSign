using Cysharp.Threading.Tasks;
using System;
using System.Collections;

[Serializable]
public class S_Reset : BaseSkillLogic
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
        // 선택한 기물을 행동 가능으로 설정
        _target.IsControllable = true;
        
        _target = null;
        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Reset();
    }
}
