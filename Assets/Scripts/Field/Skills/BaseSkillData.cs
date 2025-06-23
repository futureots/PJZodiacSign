using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseSkillData", menuName = "Scriptable Objects/Skill/BaseSkillData")]
public abstract class BaseSkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public abstract IActive CreateInstance();
}