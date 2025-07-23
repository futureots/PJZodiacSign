using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseEntityData", menuName = "Scriptable Objects/BaseEntityData")]
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
        instance.AddComponent<KingArea>();
        var entity = instance.GetOrAddComponent<Entity>();
        entity.SetEntity(this,level);

        return entity;
    }
    

}
