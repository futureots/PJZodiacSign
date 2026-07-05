using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{
    private string _defaultPath;
    /// <summary>
    /// 적 레벨 데이터
    /// </summary>
    public ItemTable itemTable;
    public EntityTable entityTable;

    public LevelTable levelTable;
    public ShopTable shopTable;

    /// <summary>
    /// 플레이어 데이터
    /// </summary>
    public PlayData playData {  get; private set; }

    public bool isModified { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _defaultPath = Path.Join(Application.persistentDataPath, "Data");
    }
    
    
    public AgentData GetPlayerAgentData()
    {

        var handEntities = new List<EntityLevelData>();
        foreach (var item in playData.handEntities)
        {
            var entityData = ConvertData(item);
            if (entityData.HasValue)
            {
                handEntities.Add(entityData.Value);
            }
        }

        var fieldEntities = new Dictionary<intVector2, EntityLevelData>();
        foreach (var item in playData.fieldEntities)
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
        foreach (var item in playData.items)
        {
            var itemData = itemTable.SearchData(item);
            items.Add(itemData);
        }

        AgentData player = new AgentData(playData.credit, handEntities, fieldEntities, items);
        return player;
    }

    public void SetData(AgentData data, int stageLevel)
    {
        List<string> itemNames = new();
        foreach (var item in data.items)
        {
            if(item) itemNames.Add(item.id);
            else itemNames.Add(null);
        }
        playData.items = itemNames;

        List<string> handEntityNames = new List<string>();
        foreach (var item in data.handEntities)
        {
            var temp = item.data.id + "+"+ item.level;
            handEntityNames.Add(temp);
        }
        playData.handEntities = handEntityNames;


        Dictionary<string,string> fieldEntities = new();
        foreach (var item in data.fieldEntities)
        {
            var posData = JsonConvert.SerializeObject(item.Key);
            fieldEntities.Add(posData, $"{item.Value.data.id}+{item.Value.level}");
        }
        playData.fieldEntities = fieldEntities;

        playData.credit = data.credit;

        playData.stageLevel = stageLevel;
    }

    public void ResetData(string fileName)
    {
        #if UNITY_EDITOR
        DeleteData(fileName);
        LoadAllData(fileName);
        #else
        DeleteSteamCloudData(fileName);
        LoadSteamCloudData(fileName);
        #endif
    }

    public void SaveAllData(string fileName)
    {
        var data = PlayData.SerializePlayerData(playData);
        EditorLogger.Print(data);
        SaveData(data, fileName);
    }

    public  void LoadAllData(string fileName)
    {
        if(TryLoadData(fileName,out var json))
        {
            playData = PlayData.DeserializePlayerData(json);
            isModified = true;
        }
        else
        {
            playData = new();
            isModified = false;
        }
    }

    private EntityLevelData? ConvertData(string csv)
    {
        var list = csv.Split('+', 2);
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

    /// <summary>
    /// data를 fileName으로 된 json파일로 저장
    /// </summary>
    public void SaveData(string data,string fileName)
    {
        
        if (!Directory.Exists(_defaultPath))
        {
            Directory.CreateDirectory(_defaultPath);
        }
        string filePath = Path.Join(_defaultPath,  $"{fileName}.Json");
        File.WriteAllText(filePath, data);
        EditorLogger.Print(data);
    }

    /// <summary>
    /// 저장된 데이터가 존재하는지 확인 후 데이터 반환
    /// </summary>
    public bool TryLoadData(string fileName, out string json)
    {
        json = null;
        if (Directory.Exists(_defaultPath))
        {
            string filePath = Path.Join(_defaultPath, $"{fileName}.Json");
            if (File.Exists(filePath))
            {
                json = File.ReadAllText(filePath);
                EditorLogger.Print(json);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// fileName으로 된 파일을 제거한다.
    /// </summary>
    public void DeleteData(string fileName)
    {
        if (Directory.Exists(_defaultPath))
        {
            string filePath = Path.Join(_defaultPath, $"{fileName}.Json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    public void LoadSteamCloudData(string fileName)
    {
        bool success = SteamRemoteStorage.FileExists(fileName);

        if (success)
        {
            var size = SteamRemoteStorage.GetFileSize(fileName);
            byte[] data = new byte[size];
            var fileSize = SteamRemoteStorage.FileRead(fileName, data, size);
            
            string json = System.Text.Encoding.UTF8.GetString(data);
            playData = PlayData.DeserializePlayerData(json);
            isModified = true;
        }
        else
        {
            playData = new();
            isModified = false;
        }
    }

    public void SaveSteamCloudData(string fileName)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(PlayData.SerializePlayerData(playData));
        int size = data.Length;
        bool success = SteamRemoteStorage.FileWrite(fileName, data, size);
    }

    public void DeleteSteamCloudData(string fileName)
    {
        SteamRemoteStorage.FileDelete(fileName);
    }
}
[Serializable]
public struct AgentData
{
    public AgentData(int credit, List<EntityLevelData> hands= null, Dictionary<intVector2, EntityLevelData> fields = null, List<ItemData> items = null)
    {
        this.credit = credit;

        handEntities = new();
        if(hands != null) this.handEntities = hands;

        fieldEntities = new();
        if (fields != null) this.fieldEntities = fields;

        this.items = new();
        if (items != null) this.items = items;
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
