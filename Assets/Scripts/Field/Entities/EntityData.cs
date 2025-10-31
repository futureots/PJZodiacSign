using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/EntityData")]
public class EntityData : AbstractData
{
    [Header("GameObject")]
    public GameObject baseEntityPrefab;
    public GameObject basicAttackEffect;
    public GameObject dissolveEffect;
    [Header("Area")]
    public List<Area> moveArea;
    public List<Area> attackArea;

    [Header("BaseStatus")]
    public int maxHp;
    public int power;

    [Header("BonusStatus")]
    public int bonusHp;
    public int bonusPower;

    [Header("Skill")]
    public BaseSkillData skill;
    public bool energy;
    public int maxEnergy;

    [Header("ObjectValue")]
    public Vector3 hpPanelPosition;

}
