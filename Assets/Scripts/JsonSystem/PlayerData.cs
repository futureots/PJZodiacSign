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
    public List<EntityData> entities;

    // 현재 위치한 지역 아이디
    public int locationId;

    //파일에서 읽어올 때 호출됨
    public PlayerData()
    {
        Debug.Log("Player Data Init");
        entities = new List<EntityData>();
        items = new();
        //entities.Add(new EntityData("chicken",0));
        locationId = 1;
    }

    // 보유중인 일회용 아이템 종류(지금은 따로 정렬 없이 리스트로 저장 => 나중에 아이템, 개수 딕셔너리 사용)
    public List<string> items;

    // 보유중인 유물 정보
    // 보유중인 재화
    // 클리어한 지역 종류 및 개수
    //public Dictionary<string,int> location = new Dictionary<string,int>();
    // 현재 보유중인 기운 개수




    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePlayerData(PlayerData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PlayerData DeserializePlayerData(string json)
    {
        if(json == null) return new PlayerData();
        return JsonConvert.DeserializeObject<PlayerData>(json,serializerSettings);
    }
    
    public void SavePlayerData(string fileName)
    {
        string data = SerializePlayerData(this);
        string path = Path.Combine(Application.dataPath+"/Data", fileName + defaultPath + ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static PlayerData LoadPlayerData(string fileName)
    {
        string path = Path.Combine(Application.dataPath+"/Data", fileName + defaultPath + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        if(data == null)
        {
            return new PlayerData();
        }
        return DeserializePlayerData(data);
    }
}
public struct EntityData
{
    public EntityData(string _name, int _level = 0)
    {
        name = _name;
        level = _level;
    }
    // 기물 이름
    public string name;
    // 기물 레벨(스탯 초기값 설정에 필요)
    public int level;

}
