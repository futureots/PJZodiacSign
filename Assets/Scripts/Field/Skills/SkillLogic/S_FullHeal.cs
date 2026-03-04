using Cysharp.Threading.Tasks;
using System;
using System.Collections;


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
        EditorLogger.Print(_target.CurHealth);
        _target.CurHealth = _target.MaxHealth;
        EditorLogger.Print(_target.CurHealth);
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_FullHeal();
    }
}
