using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseSkillData", menuName = "Scriptable Objects/Skill/BaseSkillData")]
public abstract class BaseSkillData<T> : AbstractSkillData where T : AbstractSkillInstance
{
    /*public override IActive CreateInstance() 
    {
        //var instance = new T();
        //var instance = (T)Activator.CreateInstance(typeof(T),this);
        
        //return instance;
    }*/
}

public abstract class AbstractSkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    //public abstract IActive CreateInstance();
}