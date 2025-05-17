using System;
using System.Reflection;
using UnityEngine;

public abstract class Skill : MonoBehaviour,ISkill
{

    //스킬 발동
    public abstract void Activate();
    

    public virtual bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public virtual void Reinitialize()
    {
    }
}
