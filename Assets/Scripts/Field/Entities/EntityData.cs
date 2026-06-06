using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/EntityData")]
public class EntityData : AbstractData
{
    [Header("GameObject")]
    public Entity prefab;
    
    [Header("Area")]
    public Area moveArea;
    public Area attackArea;

    [Header("BaseStatus")]
    public int maxHp;
    public int power;

    [Header("BonusStatus")]
    public int hpMultiplier;
    public int powerMultiplier;

    [Header("Skill")]
    public BaseSkillData skill;
    public int maxEnergy;
    
}
