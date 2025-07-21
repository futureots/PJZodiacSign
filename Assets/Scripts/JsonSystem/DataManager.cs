using System.Collections.Generic;
using UnityEngine;


public class DataManager : MonoBehaviour
{

    public PlayerData playerData {  get; private set; }

    /// <summary>
    /// 적 엔티티 데이터
    /// </summary>

    public ItemTable itemTable;
    
    public AgentData[] GetData()
    {
        List<AgentData> data = new List<AgentData>();

        // 아이템 데이터로 전환
        var items = new List<ItemData>();
        foreach ( var item in playerData.items)
        {
            var itemData = itemTable.SearchItem(item);
            items.Add(itemData);
        }
        AgentData player = new AgentData(playerData.credit, playerData.handEntities, playerData.fieldEntities, items);
        data.Add(player);

        AgentData enemy = new AgentData(playerData.stageLevel * 3);
        data.Add(enemy);
        return data.ToArray();
    }
    public void SetData(AgentData data, int stageLevel)
    {
        List<string> itemName = new List<string>();
        foreach (var item in data.items) 
        {
            itemName.Add(item.itemName);
        }
        playerData.items = itemName;

        playerData.handEntities = data.handEntities;
        playerData.fieldEntities = data.fieldEntities;
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
public struct AgentData
{
    public AgentData(int credit =0, List<EntityLevelData> hands= null, Dictionary<int, EntityLevelData> fields = null, List<ItemData> items = null)
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
    public List<ItemData> items;
}

