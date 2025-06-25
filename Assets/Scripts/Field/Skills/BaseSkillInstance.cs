using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseSkillInstance : AbstractSkillInstance 
{
    
    protected BaseSkillData data;

    public BaseSkillInstance(BaseSkillData data)
    {
        this.data = data;
        Debug.Log(data);
    }
    

}

public abstract class AbstractSkillInstance : IActive
{
    public string skillName;
    public string skillDescription;

    
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

    public abstract void Activate();
    public abstract bool IsValidInput(FieldInfo field);

    public abstract bool IsActable();

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
