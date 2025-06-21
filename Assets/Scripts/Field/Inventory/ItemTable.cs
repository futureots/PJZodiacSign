using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : ScriptableObject
{
    [SerializeField] List<ItemSetData> itemTableData;
    public List<ItemSetData> itemTable
    {
        get
        {
            return itemTableData;
        }
    }

    public ItemData GetRandomItem()
    {
        float sum = itemTable.Sum(x => x.weight);
        float rand = Random.Range(0, sum);
        foreach (var item in itemTable)
        {
            rand -= item.weight;
            if (rand <= 0)
            {
                return item.itemData;
            }
        }
        return itemTable.Last().itemData;
    }
    public ItemData SearchItem(string name)
    {
        var item = itemTable.Find(x => x.itemData.itemName.Equals(name));
        Debug.Log(item.itemData.itemName);
        if (item == null) return null;
        return item.itemData;
    }
}
[System.Serializable]
public struct ItemSetData
{
    public ItemData itemData;
    public float weight;

    public override bool Equals(object obj)
    {
        return obj is ItemSetData data &&
               EqualityComparer<ItemData>.Default.Equals(itemData, data.itemData) &&
               weight == data.weight;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(itemData, weight);
    }

    public static bool operator==(ItemSetData a, ItemSetData b)
    {
        return a.itemData.Equals(b.itemData);
    }
    public static bool operator !=(ItemSetData a, ItemSetData b)
    {
        return !a.itemData.Equals(b.itemData);
    }
}
