using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ShopTable;

[CreateAssetMenu(fileName = "ShopTable", menuName = "Scriptable Objects/ShopTable")]
public class ShopTable : ScriptableObject
{
    public List<DataSet<ItemData>> itemList;
    public List<DataSet<EntityData>> entityList;


    public List<ItemData> GetRandomItem(int count)
    {
        List<ItemData> list = new List<ItemData>();
        var table = new List<DataSet<ItemData>>(itemList);
        for (int i = 0; i < count; i++)
        {
            var sum = table.Sum(x => x.weight);
            var rand = UnityEngine.Random.Range(0, sum);
            foreach (var item in table)
            {
                rand -= item.weight;
                if (rand <= 0)
                {
                    table.Remove(item);
                    list.Add(item.data);
                    break;
                }
            }
        }

        return list;
    }
    public List<EntityData> GetRandomEntity(int count)
    {
        List<EntityData> list = new List<EntityData>();
        var table = new List<DataSet<EntityData>>(entityList);
        for (int i = 0; i < count; i++)
        {
            var sum = table.Sum(x => x.weight);
            var rand = UnityEngine.Random.Range(0, sum);
            foreach (var item in table)
            {
                rand -= item.weight;
                if (rand <= 0)
                {
                    table.Remove(item);
                    list.Add(item.data);
                    break;
                }
            }
        }

        return list;
    }
}
[System.Serializable]
public struct DataSet<T> where T : AbstractData
{
    public T data;
    public float weight;

    public override bool Equals(object obj)
    {
        return obj is DataSet<T> data &&
               EqualityComparer<T>.Default.Equals(this.data, data.data) &&
               weight == data.weight;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(data, weight);
    }

    public static bool operator ==(DataSet<T> a, DataSet<T> b)
    {
        return a.data.Equals(b.data);
    }
    public static bool operator !=(DataSet<T> a, DataSet<T> b)
    {
        return !a.data.Equals(b.data);
    }
}
