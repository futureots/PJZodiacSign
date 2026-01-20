using System;
using System.Collections;

[Serializable]
public class S_Enhance : BaseSkillLogic
{
    Entity target;
    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        var list = StageManager.Instance.field.GetEntities();
        isContinued = false;
        Entity _target = null;
        Action<Entity> action = (x) =>
        {
            _target = x;
        };
        Action<bool> conti = (flag) => { isContinued = flag; };
        yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
        if (!isContinued)
        {
            callback?.Invoke(false);
            yield break;
        }
        target = _target;
        callback?.Invoke(true);
    }

    public override IEnumerator ExecuteSkill()
    {
        target.Level += 1;
        target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Enhance();
    }
}
