using System;
using System.Collections.Generic;
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
        // 가중치를 이용해 아이템 데이터 반환
        foreach (var item in itemList)
        {
            list.Add(item.data);
        }
        return list;
    }
    public List<EntityData> GetRandomEntity(int count)
    {
        List<EntityData> list = new List<EntityData>();
        // 가중치를 이용해 아이템 데이터 반환
        foreach (var item in entityList)
        {
            list.Add(item.data);
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
