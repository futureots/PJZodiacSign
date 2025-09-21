using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    public static Entity CreateEntity(EntityData data, int level=0)
    {
        var instance = Instantiate(data.baseEntityPrefab);
        var entity = instance.AddComponent<Entity>();
        // 스탯 정의
        entity.name = data.id;
        entity.InitializeEntity(data, level);
        
        instance.AddComponent<BuffManager>();

        // 범위 정의
        var area = instance.AddComponent<AreaComponent>();
        area.SetArea(data.moveArea, data.attackArea);

        if (data.energy)
        {
            var energy = instance.AddComponent<EnergyComponent>();
            energy.Initialize(data.maxEnergy);
        }
        
        // 스킬 정의
        if (data.skill)
        {
            var skill = instance.AddComponent<SkillComponent>();
            skill.SetupSkill(data.skill);
        }
        return entity;
    }
}
