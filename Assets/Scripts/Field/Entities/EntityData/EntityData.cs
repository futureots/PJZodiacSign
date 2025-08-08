using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityData : AbstractData
{
    public GameObject baseEntityPrefab;


    [Header("BaseStatus")]
    public int maxHp;
    public int power;

    [Header("BonusStatus")]
    public int bonusHp;
    public int bonusPower;

    [Header("Skill")]
    public BaseSkillData skill;
    public int skillCost;

    [Header("ObjectValue")]
    public Vector3 hpPanelPosition;
    public abstract Entity CreateEntity(int level = 0);

    protected Entity CreateInstance()
    {
        var instance = Instantiate(baseEntityPrefab);
        var entity = instance.GetOrAddComponent<Entity>();
        return entity;
    }
}
