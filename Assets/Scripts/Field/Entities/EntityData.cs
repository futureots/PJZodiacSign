using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData : ScriptableObject
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

    public virtual Entity CreateEntity(int level = 0)
    {
        var instance = Instantiate(baseEntityPrefab);

        // 기물 공격, 이동 범위 세팅
        instance.AddComponent<ObjectArea>();
        var entity = instance.GetOrAddComponent<Entity>();
        entity.SetEntity(this);

        // 기물 스탯 세팅
        entity.power = power + bonusPower * level;
        entity.maxHp = maxHp + bonusHp * level;

        // 기물 스킬 세팅(스킬이 엔티티 전용 스킬이면 시전자 할당, 아니면 할당X)
        entity.SetSkill(skill);

        return entity;
    }
    

}
