using System.Collections.Generic;
using UnityEngine;


public class DataManager : Singleton<DataManager>
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



    public void LoadAllData(string fileName)
    {
        playerData = PlayerData.LoadPlayerData(fileName);
    }


    public void SaveAllData(string fileName)
    {
        playerData.SavePlayerData(fileName);
    }
    public override void Init()
    {
        if (transform.parent != null || transform.root != null)                                                             //싱글톤 오브젝트가 파괴되지 않도록 처리
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        LoadAllData("Data");
    }
}
public struct AgentData
{
    public AgentData(int credit =0, List<EntityData> hands= null, Dictionary<int, EntityData> fields = null, List<ItemData> items = null)
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
    public List<EntityData> handEntities;
    public Dictionary<int, EntityData> fieldEntities;
    public List<ItemData> items;
}

