using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{

    /// <summary>
    /// 플레이어 데이터
    /// </summary>
    public PlayData playerData {  get; private set; }
    /// <summary>
    /// 적 레벨 데이터
    /// </summary>
    public LevelTable enemyData;

    public ItemTable itemTable;
    public EntityTable entityTable;
    
    public AgentData[]  GetData()
    {
        List<AgentData> data = new List<AgentData>();

        var handEntities = new List<EntityLevelData>();
        foreach (var item in playerData.handEntities)
        {
            var entityData = ConvertData(item);
            if (entityData.HasValue)
            {
                handEntities.Add(entityData.Value);
            }
        }

        var fieldEntities = new Dictionary<intVector2, EntityLevelData>();
        foreach (var item in playerData.fieldEntities)
        {
            var entityData = ConvertData(item.Value);
            if (entityData.HasValue)
            {
                var pos = JsonConvert.DeserializeObject<intVector2>(item.Key);
                fieldEntities.Add(pos, entityData.Value);
            }
        }

        // 아이템 데이터로 전환
        var items = new List<ItemData>();
        foreach (var item in playerData.items)
        {
            var itemData = itemTable.SearchData(item);
            items.Add(itemData);
        }
        Debug.Log(items.Count);

        AgentData player = new AgentData(playerData.credit, handEntities, fieldEntities, items);
        data.Add(player);

        AgentData enemy = enemyData.GetLevelData(playerData.stageLevel);

        data.Add(enemy);
        return data.ToArray();
    }
    public void SetData(AgentData data, int stageLevel)
    {
        List<string> itemNames = new();
        foreach (var item in data.items)
        {
            itemNames.Add(item.id);
        }
        playerData.items = itemNames;

        List<string> handEntityNames = new List<string>();
        foreach (var item in data.handEntities)
        {
            var temp = item.data.id + "+"+ item.level;
            handEntityNames.Add(temp);
        }
        playerData.handEntities = handEntityNames;


        Dictionary<string,string> fieldEntities = new();
        foreach (var item in data.fieldEntities)
        {
            var posData = JsonConvert.SerializeObject(item.Key);
            fieldEntities.Add(posData, $"{item.Value.data.id}+{item.Value.level}");
        }
        playerData.fieldEntities = fieldEntities;


        playerData.credit = data.credit;

        playerData.stageLevel = stageLevel;

    }


    public void LoadAllData(string fileName)
    {
        playerData = PlayData.LoadPlayerData(fileName);
    }


    public void SaveAllData(string fileName)
    {
        playerData.SavePlayerData(fileName);
    }

    EntityLevelData? ConvertData(string csv)
    {
        
        var list = csv.Split('+',2);
        if (list.Length == 1)
        {
            return new EntityLevelData(entityTable.SearchData(list[0]));
        }
        else if (list.Length == 2)
        {
            return new EntityLevelData(entityTable.SearchData(list[0]), int.Parse(list[1]));
        }
        else return null;
    }

}
[System.Serializable]
public struct AgentData
{
    public AgentData(int credit =0, List<EntityLevelData> hands= null, Dictionary<intVector2, EntityLevelData> fields = null, List<ItemData> items = null)
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
    public Dictionary<intVector2, EntityLevelData> fieldEntities;
    public List<ItemData> items;
}
[System.Serializable]
public struct EntityLevelData
{
    public EntityLevelData(Entity entity)
    {
        data = entity.baseData;
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
