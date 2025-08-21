
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTable", menuName = "Scriptable Objects/EnemyTable")]
public class EnemyTable : ScriptableObject
{
    public int credit;
    public List<EntityLevelData> entities;
    public List<FieldEntityLevelData> fieldEntities;



    public AgentData GetAgentData()
    {
        Dictionary<int,EntityLevelData> fields = new Dictionary<int,EntityLevelData>();
        foreach (var entity in fieldEntities)
        {
            fields.Add(entity.fieldPos.Encode(), entity.entityData);
        }
        List<EntityLevelData> hands = new List<EntityLevelData>(entities);
        return new AgentData(credit, hands,fields);
    }
}
[System.Serializable]
public struct FieldEntityLevelData
{
    public EntityLevelData entityData;
    public intVector2 fieldPos;
}