using UnityEngine;


public abstract class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;
    public abstract IActive CreateInstance();
}