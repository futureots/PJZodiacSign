using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class BaseSkillLogic
{
    protected SkillComponent component;
    protected bool isContinued = false;
    public void SetSkillComponent(SkillComponent skillComponent) => this.component = skillComponent;

    public abstract UniTask<bool> InputSkill(IInput input);

    public virtual IEnumerator ExecuteSkill() { yield break; }

    public virtual bool IsValuable()
    {
        return true;
    }

    public abstract BaseSkillLogic Clone();
    
}
