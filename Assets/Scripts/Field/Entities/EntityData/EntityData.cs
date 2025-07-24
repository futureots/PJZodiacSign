using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    public GameObject baseEntityPrefab;
    public string id;
    

    [Header("BaseStatus")]
    public int maxHp;
    public int power;

    [Header("BonusStatus")]
    public int bonusHp;
    public int bonusPower;

    [Header("Skill")]
    public BaseSkillData skill;
    public int skillCost;

    public abstract Entity CreateEntity(int level = 0);

    protected Entity CreateInstance()
    {
        var instance = Instantiate(baseEntityPrefab);
        var entity = instance.GetOrAddComponent<Entity>();
        return entity;
    }
}
