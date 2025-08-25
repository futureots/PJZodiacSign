using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

/// <summary>
/// 플레이어의 데이터 저장 클래스 json 저장 및 불러오기 가능
/// </summary>
public class PlayerData
{
    [NonSerialized]
    static string defaultPath = "Player";

    // 현재 보유중인 기물 정보
    public Dictionary<int, EntityLevelHolder> fieldEntities;

    public List<EntityLevelHolder> handEntities;

    // 현재 위치한 지역 아이디
    public int stageLevel;

    // 현재 보유중인 (아이템 정보,개수)
    public Dictionary<int, string> items;

    public int credit;

    //파일에서 읽어올 때 호출됨
    public PlayerData()
    {
        //Debug.Log("Player Data Init");
        fieldEntities = new Dictionary<int, EntityLevelHolder>();
        // 인코딩으로 int 값으로 변환
        handEntities = new List<EntityLevelHolder>();
        items = new();
        stageLevel = 1;
        credit = 100;

        #region DebugData
        /*
        items.Add("Warp");
        items.Add("Warp");
        items.Add("Warp");
        fieldEntities.Add(2050,new EntityLevelHolder("WhiteKing", 1));
        handEntities.Add(new EntityLevelHolder("WhiteRook", 0));
        */
        #endregion
    }






    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePlayerData(PlayerData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PlayerData DeserializePlayerData(string json)
    {
        if (json == null) return new PlayerData();
        return JsonConvert.DeserializeObject<PlayerData>(json, serializerSettings);
    }

    public void SavePlayerData(string fileName)
    {
        string data = SerializePlayerData(this);
        //string data = JsonUtility.ToJson(this);
        string path = Path.Combine(Application.dataPath + "/Data", fileName + defaultPath + ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static PlayerData LoadPlayerData(string fileName)
    {
        string path = Path.Combine(Application.dataPath + "/Data", fileName + defaultPath + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
            Debug.Log(data);
        }
        if (data == null)
        {
            return new PlayerData();
        }
        //return JsonUtility.FromJson<PlayerData>(data);
        return DeserializePlayerData(data);
    }
}

[Serializable]
public struct EntityLevelHolder
{
    public EntityLevelHolder(string _name, int _level = 0)
    {
        entity = _name;
        level = _level;
    }
    public EntityLevelHolder(Entity entity)
    {
        this.entity = entity.id;
        level = entity.Level;
    }
    // 기물 이름
    public string entity;
    // 기물 레벨(스탯 초기값 설정에 필요)
    public int level;
}
