using UnityEngine;


public abstract class BaseSkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public abstract IActive CreateInstance();
}