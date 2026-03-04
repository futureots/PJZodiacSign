using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopTable", menuName = "Scriptable Objects/ShopTable")]
public class ShopTable : ScriptableObject
{
    public List<DataSet<ItemData>> itemList;
    public List<DataSet<EntityData>> entityList;

    /// <summary>
    /// 무작위 n개(중복 불가)의 아이템 데이터를 리스트 형식으로 반환
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 무작위 n개(중복 불가)의 기물 데이터를 리스트 형식으로 반환
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 표시된 기물 중 무작위로 구매 가능한 기물 구매 및 성공 여부 반환
    /// </summary>
    /// <param name="credit"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetBuyableEntity(int credit, out EntityData data)
    {
        data = null;
        var table = entityList.Where(x => x.data.normalPrice <= credit);
        var sum = table.Sum(x => x.weight);
        var rand = UnityEngine.Random.Range(0, sum);
        foreach (var item in table)
        {
            rand -= item.weight;
            if (rand <= 0)
            {
                data = item.data;
                return true;
            }
        }
        return false;
    }
}
[Serializable]
public struct DataSet<T> where T : AbstractData
{
    public T data;
    /// <summary>
    /// 가중치
    /// </summary>
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
