using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class MapData
{
    [NonSerialized]
    protected static string defaultPath = "Map";

    public MapData()
    {
        map = new();

        /*// debug
        var entities = new List<EntityData>();
        entities.Add(new EntityData("chicken", 0));
        entities.Add(new EntityData("chicken", 0));
        Location temp = new Location(1,entities);
        map.Add(temp.id,temp);*/
    }

    
    public Dictionary<int, Location> map;


    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializeMapData(MapData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static MapData DeserializeMapData(string json)
    {
        if (json == null) return new MapData();
        return JsonConvert.DeserializeObject<MapData>(json, serializerSettings);
    }

    public void SaveMapData(string fileName)
    {
        string data = SerializeMapData(this);
        string path = Path.Combine(Application.dataPath + "/Data", fileName + defaultPath+ ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static MapData LoadMapData(string fileName)
    {
        string path = Path.Combine(Application.dataPath + "/Data", fileName + defaultPath + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        if (data == null)
        {
            return new MapData();
        }
        return DeserializeMapData(data);
    }

}
[System.Serializable]
public struct Location
{
    public Location(int id, List<EntityData> entities)
    {
        this.id = id;
        enemyList = entities.ToArray();
    }


    // ¸Ê ¾ÆÀÌµð
    public int id { get; private set; }

    // Àû Á¾·ù
    public EntityData[] enemyList;

    // ¸Ê Á¾·ù
}