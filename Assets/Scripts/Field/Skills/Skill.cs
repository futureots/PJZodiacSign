using Battle;

using System;
using UnityEngine;

public class Skill : MonoBehaviour,ISkill
{

    //스킬 발동
    public virtual void Activate()
    {
        Debug.Log( " Skill Active");
    }

    public void Reinitialize()
    {
    }
}
