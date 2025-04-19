using Battle;

using System;
using UnityEngine;

public class Skill : MonoBehaviour,ISkill
{

    //스킬 발동
    public virtual void Activate()
    {
<<<<<<< Updated upstream
        try
        {
            entity.MoveTo(tile);
            Debug.Log(tile.name + " Skill Active");
        }
        catch 
        {
            throw new Exception("Skill values arenot completed");
        }
        
    }
=======
>>>>>>> Stashed changes

        Debug.Log( " Skill Active");
    }

    public void Reinitialize()
    {
    }
}
