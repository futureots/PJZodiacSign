using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

/// <summary>
/// 플레이어의 데이터 저장 클래스 json 저장 및 불러오기 가능
/// </summary>
public class PlayData
{
    [NonSerialized]
    static string defaultPath = "Player";

    /// <summary>
    /// 필드에 배치한 플레이어 기물 정보
    /// </summary>
    public Dictionary<string, string> fieldEntities;
    /// <summary>
    /// 배치하지 않았지만 보유한 플레이어 기물 정보
    /// </summary>
    public List<string> handEntities;

    // 현재 위치한 지역 아이디
    public int stageLevel;

    // 현재 보유중인 (아이템 정보,개수)
    public List<string> items;

    public int credit;

    //파일에서 읽어올 때 호출됨
    public PlayData()
    {
        fieldEntities = new Dictionary<string, string>();
        // 인코딩으로 int 값으로 변환
        handEntities = new List<string>();
        items = new();
        stageLevel = 1;
        credit = 100;

        #region DebugData
        /*
        items.Add(null);
        items.Add("Enhence");
        items.Add("Warp");
        fieldEntities[JsonConvert.SerializeObject(new intVector2(0,0))] = "WhiteKing";
        handEntities.Add("WhitePawn+2");
        */
        #endregion
    }

    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePlayerData(PlayData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PlayData DeserializePlayerData(string json)
    {
        if (json == null) return new PlayData();
        return JsonConvert.DeserializeObject<PlayData>(json, serializerSettings);
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
    public static PlayData LoadPlayerData(string fileName)
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
            return new PlayData();
        }
        //return JsonUtility.FromJson<PlayerData>(data);
        return DeserializePlayerData(data);
    }
}

