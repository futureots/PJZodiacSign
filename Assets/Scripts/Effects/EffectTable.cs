using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectTable", menuName = "Scriptable Objects/EffectTable")]
public class EffectTable : ScriptableObject
{
    public List<GameObject> effectList;

    public Dictionary<string, GameObject> GetTableDictionary()
    {
        var data = new Dictionary<string, GameObject>();

        foreach (var effect in effectList)
        {
            data.Add(effect.name, effect);
        }

        return data;
    }
    
    
}
