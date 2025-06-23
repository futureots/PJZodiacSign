using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseSkillInstance<T> : AbstractSkillInstance where T : BaseSkillData
{
    
    protected T data;

    public BaseSkillInstance() { }
    public BaseSkillInstance(T data)
    {
        this.data = data;
        Debug.Log(data);
    }
    

}

public abstract class AbstractSkillInstance : IActive
{
    public string skillName;
    public string skillDescription;

    public abstract void Activate();
    protected Action<bool> callback;
    public bool ExecuteSequence()
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

    public virtual bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public virtual bool IsActable()
    {
        return true;
    }

    public abstract void Reinitialize();

    public void AddCallback(Action<bool> func)
    {
        callback += func;
    }

    public void ClearCallback()
    {
        callback = null;
    }
}
