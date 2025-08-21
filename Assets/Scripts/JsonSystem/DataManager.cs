using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DataManager : MonoBehaviour
{

    public PlayerData playerData {  get; private set; }
    public LevelTable enemyData;
    /// <summary>
    /// 적 엔티티 데이터
    /// </summary>

    public ItemTable itemTable;
    public EntityTable entityTable;
    
    public AgentData[]  GetData()
    {
        List<AgentData> data = new List<AgentData>();

        // 아이템 데이터로 전환
        var items = new Dictionary<int, ItemData>();
        foreach ( var item in playerData.items)
        {
            var itemData = itemTable.SearchData(item.Value);
            items.Add(item.Key, itemData);
        }

        var handEntities = new List<EntityLevelData>();
        foreach (var item in playerData.handEntities)
        {
            var entityData = new EntityLevelData(entityTable.SearchData(item.entity),item.level);
            handEntities.Add(entityData);
        }

        var fieldEntities = new Dictionary<int, EntityLevelData>();
        foreach (var item in playerData.fieldEntities)
        {
            var entityData = new EntityLevelData(entityTable.SearchData(item.Value.entity), item.Value.level);
            fieldEntities.Add(item.Key, entityData);
        }

        AgentData player = new AgentData(playerData.credit, handEntities, fieldEntities, items);
        data.Add(player);


        AgentData enemy = enemyData.GetLevelData(playerData.stageLevel);

        data.Add(enemy);
        return data.ToArray();
    }
    public void SetData(AgentData data, int stageLevel)
    {
        Dictionary<int, string> itemNames = new();
        foreach (var item in data.items)
        {
            itemNames.Add(item.Key, item.Value.id);
        }
        playerData.items = itemNames;

        List<EntityLevelHolder> handEntityNames = new List<EntityLevelHolder>();
        foreach (var item in data.handEntities)
        {
            EntityLevelHolder temp = new EntityLevelHolder(item.data.id, item.level);
            handEntityNames.Add(temp);
        }
        playerData.handEntities = handEntityNames;


        Dictionary<int,EntityLevelHolder> fieldEntities = new Dictionary<int,EntityLevelHolder>();
        foreach (var item in data.fieldEntities)
        {
            fieldEntities.Add(item.Key, new EntityLevelHolder(item.Value.data.id, item.Value.level));
        }
        playerData.fieldEntities = fieldEntities;


        playerData.credit = data.credit;

        playerData.stageLevel = stageLevel;

    }


    public void LoadAllData(string fileName)
    {
        playerData = PlayerData.LoadPlayerData(fileName);
    }


    public void SaveAllData(string fileName)
    {
        playerData.SavePlayerData(fileName);
    }


}
[System.Serializable]
public struct AgentData
{
    public AgentData(int credit =0, List<EntityLevelData> hands= null, Dictionary<int, EntityLevelData> fields = null, Dictionary<int,ItemData> items = null)
    {
        this.credit = credit;

        if(hands == null) handEntities = new();
        else this.handEntities = hands;
        
        if (fields== null) fieldEntities = new();
        else this.fieldEntities = fields;
        
        if(items == null) this.items = new();
        else this.items = items;
    }

    public int credit;
    public List<EntityLevelData> handEntities;
    public Dictionary<int, EntityLevelData> fieldEntities;
    public Dictionary<int,ItemData> items;
}
[System.Serializable]
public struct EntityLevelData
{
    public EntityLevelData(Entity entity)
    {
        data = entity.data;
        level = entity.Level;
    }
    public EntityLevelData(EntityData entityData, int level = 0)
    {
        data = entityData;
        this.level = level;
    }

    public EntityData data;
    public int level;
}
