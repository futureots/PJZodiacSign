using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// 플레이어의 데이터 저장 클래스 json 저장 및 불러오기 가능
/// </summary>
public class PlayerData
{
    // 현재 보유중인 기물 정보
    public List<EntityData> entities;

    public PlayerData()
    {
        entities = new List<EntityData>();
        entities.Add(new EntityData("chicken",0));
    }


    // 보유중인 유물 정보
    // 보유중인 재화
    // 클리어한 지역 종류 및 개수
    //public Dictionary<string,int> location = new Dictionary<string,int>();
    // 현재 보유중인 기운 개수
    // 보유중인 일회용 아이템 종류와 개수



    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePartyData(PlayerData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PlayerData DeserializePartyData(string json)
    {
        if(json == null) return new PlayerData();
        return JsonConvert.DeserializeObject<PlayerData>(json,serializerSettings);
    }
    
    public void SavePlayerData(string fileName)
    {
        string data = SerializePartyData(this);
        string path = Path.Combine(Application.dataPath+"/Data", fileName + ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static PlayerData LoadPlayerData(string fileName)
    {
        string path = Path.Combine(Application.dataPath+"/Data", fileName + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        if(data == null)
        {
            return new PlayerData();
        }
        return DeserializePartyData(data);
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
