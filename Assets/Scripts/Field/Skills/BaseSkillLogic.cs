using Cysharp.Threading.Tasks;
using System;
using System.Collections;

[Serializable]
public abstract class BaseSkillLogic
{
    protected SkillComponent component;
    protected bool isContinued = false;
    public void SetSkillComponent(SkillComponent skillComponent) => this.component = skillComponent;

    public abstract UniTask<bool> InputSkill(IInput input);

    public virtual IEnumerator ExecuteSkill() { yield break; }

    public abstract BaseSkillLogic Clone();
}
