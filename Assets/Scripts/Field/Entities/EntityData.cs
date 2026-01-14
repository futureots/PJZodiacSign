using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/EntityData")]
public class EntityData : AbstractData
{
    [Header("GameObject")]
    public Entity prefab;
    public GameObject basicAttackEffect;        // TODO: Effect Pool로 별도 처리
    public GameObject dissolveEffect;           // 접근은 Effect ID나 기타 객체로 처리
    
    [Header("Area")]
    public List<Area> moveArea;
    public List<Area> attackArea;

    [Header("BaseStatus")]
    public int maxHp;
    public int power;

    [Header("BonusStatus")]
    public int hpMultiplier;
    public int powerMultiplier;

    [Header("Skill")]
    public BaseSkillData skill;
    public int maxEnergy;

    [Header("ObjectValue")]
    public Vector3 hpPanelPosition;
}
