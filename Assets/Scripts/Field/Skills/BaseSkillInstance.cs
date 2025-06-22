using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseSkillInstance<T> : AbstractSkillInstance where T : AbstractSkillData
{
    public Action<bool> callback;
    protected T data;

    public BaseSkillInstance() { }
    public BaseSkillInstance(T data)
    {
        this.data = data;
        Debug.Log(data);
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

public abstract class AbstractSkillInstance : IActive
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
