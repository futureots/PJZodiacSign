using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseSkill<T> : AbstractSkill where T : AbstractSkillData
{
    public Action<bool> callback;
    protected T data;

    public BaseSkill(T data)
    {
        this.data = data;
    }
    public override bool ExecuteSequence()
    {
        var isActable = IsActable();
        if (isActable)
        {
            Activate();
            Reinitialize();
        }
        callback?.Invoke(isActable);
        return isActable;
    }

}

public abstract class AbstractSkill : IActive
{
    public string skillName;
    public string skillDescription;

    public abstract void Activate();

    public abstract bool ExecuteSequence();

    public virtual bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public virtual bool IsActable()
    {
        return true;
    }

    public abstract void Reinitialize();
}
