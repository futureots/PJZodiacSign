using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntitySkill : MonoBehaviour, ISkill
{
    
    public Entity owner
    {
        get
        {
            return GetComponent<Entity>();
        }
    }

    
    public void Activate()
    {
        owner.AddBuff(new Protect(), 3);
    }

    public bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public void Reinitialize()
    {
        
    }
}
