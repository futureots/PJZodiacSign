using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class PartyData
{
    public PartyEntity[] Entities;

    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePartyData(PartyData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PartyData DeserializePartyData(string json)
    {
        if(json == null) return null;
        return JsonConvert.DeserializeObject<PartyData>(json,serializerSettings);
    }
    
    public void SavePartyData(string fileName)
    {
        string data = SerializePartyData(this);
        string path = Path.Combine(Application.dataPath+"/Data", fileName + ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static PartyData LoadPartyData(string fileName)
    {
        string path = Path.Combine(Application.dataPath+"/Data", fileName + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        if(data == null)
        {
            return new PartyData();
        }
        return DeserializePartyData(data);
    }
}
public struct PartyEntity
{
    public PartyEntity(string name, int entityLevel = 1)
    {
        this.entityId = name;
        this.entityLevel = entityLevel;
    }
    public string entityId;
    public int entityLevel;
}
