using Condition;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;
    public List<ConditionData> conditions;

    public abstract IActive CreateInstance();
}