using System;
using System.Collections.Generic;
using Newtonsoft.Json;


/// <summary>
/// 플레이어의 데이터 저장 클래스 json 저장 및 불러오기 가능
/// </summary>
public class PlayData
{
    /// <summary>
    /// 필드에 배치한 플레이어 기물 정보
    /// </summary>
    public Dictionary<string, string> fieldEntities;

    /// <summary>
    /// 배치하지 않은 보유중인 플레이어 기물 정보
    /// </summary>
    public List<string> handEntities;

    /// <summary>
    /// 현재 보유중인 아이템 정보
    /// </summary>
    public List<string> items;

    /// <summary>
    /// 보유 재화
    /// </summary>
    public int credit;


    // 현재 위치한 지역 아이디
    public int stageLevel;

    public float time;

    
    public DateTime startTime;

    //파일에서 읽어올 때 호출됨
    public PlayData()
    {
        fieldEntities = new Dictionary<string, string>();
        handEntities = new List<string>();
        items = new();
        stageLevel = 1;
        // 시작 크레딧
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

    public void Initialize()
    {
        startTime = DateTime.Now;
    }
    public void End()
    {
        var endTime = DateTime.Now;
        var timeGap = endTime - startTime;
        
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
}

