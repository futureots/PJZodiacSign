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
    
    public void SavePartyData()
    {
        string data = SerializePartyData(this);
        string path = Path.Combine(Application.dataPath, "Text.Json");
        File.WriteAllText(path, data);
    }
    public static PartyData LoadPartyData()
    {
        string path = Path.Combine(Application.dataPath, "Text.Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        return DeserializePartyData(data);
    }
}
public struct PartyEntity
{
    public PartyEntity(intVector2 pos, string name)
    {
        this.pos = pos;
        this.entityId = name;
    }
    public intVector2 pos;
    public string entityId;
}
