using System.Collections.Generic;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{

    public PlayerData playerData {  get; private set; }

    public MapData mapData { get; private set; }
    
    /// <summary>현재 플레이어 위치한 지역 정보(적 리스트, 맵 종류)</summary>
    public Location curLocation;



    public void LoadAllData(string fileName)
    {
        playerData = PlayerData.LoadPlayerData(fileName);
        mapData = MapData.LoadMapData(fileName);
        //Debug.Log(playerData.entities.Count);
        // null 확인 필요 할수도 있음
        curLocation = mapData.map[playerData.locationId];
    }


    public void SaveAllData(string fileName)
    {
        playerData.SavePlayerData(fileName);
        mapData.SaveMapData(fileName);
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

