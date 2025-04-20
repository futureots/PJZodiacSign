using Battle;

using System;
using System.Reflection;
using UnityEngine;

public class Skill : MonoBehaviour,ISkill
{

    //스킬 발동
    public virtual void Activate()
    {
        Debug.Log( " Skill Active");
    }
    

    public bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public void Reinitialize()
    {
    }
}
