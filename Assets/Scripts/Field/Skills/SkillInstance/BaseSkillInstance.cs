using System;
using System.Reflection;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class BaseSkillInstance<T> : AbstractSkillInstance where T : BaseSkillData
{
    
    protected T data;

    public BaseSkillInstance(T data)
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
            Debug.Log("SKILL 사용");
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
            var attr = item.GetCustomAttribute<SkillTargetAttribute>();
            if (attr != null)
            {
                var check = IsValidInput(item);
                Debug.Log("실행 가능 여부 : " + check);
                if (!check)
                {
                    return false;
                }
            }
            else continue;
        }
        return true;
    }

    public abstract void Reinitialize();
    public abstract bool CanSkillInput(Field field);
    public abstract bool SetSkillInput(Field field);
    public void AddCallback(Action<bool> func)
    {
        callback += func;
    }

    public void ClearCallback()
    {
        callback = null;
    }
}
