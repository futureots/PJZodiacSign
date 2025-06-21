using System;
using System.Reflection;
using UnityEngine;

public abstract class Skill : MonoBehaviour,IActive
{
    public Action<bool> callback;
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
    //스킬 발동
    public abstract void Activate();
    
    public virtual bool IsActable()
    {
        return true;
    }
    public virtual bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public virtual void Reinitialize()
    {
    }
}
