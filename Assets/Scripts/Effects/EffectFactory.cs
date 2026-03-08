using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : Singleton<EffectFactory>
{
    public EffectTable dataTable;

    private Dictionary<string, GameObject> _dataDictionary = new();

    private void Start()
    {
        Initialize(dataTable);
    }

    public void Initialize(EffectTable table)
    {
        dataTable = table;
        _dataDictionary = dataTable.GetTableDictionary();
    }
    
    public GameObject RequestEffect(string objName, Vector3 position, Vector3 scale)
    {
        GameObject obj = Instantiate(_dataDictionary[objName], position, Quaternion.identity);
        obj.transform.localScale = Vector3.Scale(obj.transform.localScale, scale);
        
        return obj;
    }
}
