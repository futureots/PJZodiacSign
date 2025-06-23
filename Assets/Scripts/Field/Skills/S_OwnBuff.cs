using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class S_OwnBuff : Skill
{
    
    public Entity owner
    {
        get
        {
            return GetComponent<Entity>();
        }
    }

    
    public override void Activate()
    {
        owner.AddBuff(new Protect(), 3);
    }

    public override bool IsValidInput(FieldInfo field)
    {
        return true;
    }

}
