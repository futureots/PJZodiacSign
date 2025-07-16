using System;
using System.Reflection;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class BaseSkillInstance : AbstractSkillInstance 
{
    
    protected BaseSkillData data;

    public BaseSkillInstance(BaseSkillData data)
    {
        this.data = data;
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

    public bool IsActable()
    {
        foreach (var item in GetType().GetFields())
        {
            var attr = item.GetCustomAttribute(typeof(SkillTargetAttribute));
            if (attr != null)
            {
                if (!IsValidInput(item))
                {
                    return false;
                }
            }
        }
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
