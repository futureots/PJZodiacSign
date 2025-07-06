using System.Collections.Generic;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{

    public PlayerData playerData {  get; private set; }

    /// <summary>
    /// 적 엔티티 데이터
    /// </summary>
    
    
    
    



    public void LoadAllData(string fileName)
    {
        playerData = PlayerData.LoadPlayerData(fileName);
        //mapData = MapData.LoadMapData(fileName);
        //Debug.Log(playerData.entities.Count);
        //curLocation = mapData.map[playerData.stageLevel];
    }


    public void SaveAllData(string fileName)
    {
        playerData.SavePlayerData(fileName);
        //mapData.SaveMapData(fileName);
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

