using Mono.Cecil;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 유저가 들고 있는 아이템
    /// </summary>
    public Dictionary<int,ItemInstance> items { get; private set; }
    public Action<int, ItemInstance> OnItemChanged;
    public int capacity;
    private void Awake()
    {
        items = new Dictionary<int, ItemInstance>();
    }

    public bool AddItem(ItemInstance instance)
    {
        // 용량 개수 만큼 확인 및 빈 공간에 추가
        if (CanAddItem(out int index))
        {
            items.Add(index, instance);
            OnItemChanged?.Invoke(index, instance);
            return true;
        }
        
        // 용량 부족
        return false;
    }
    /// <summary>
    /// 인벤토리에 아이템 추가
    /// </summary>
    /// <returns></returns>
    public bool AddItem(ItemData data)
    {
        return AddItem(data.CreateInstance());
    }

    /// <summary>
    /// 인벤토리 아이템 제거
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        if(items.ContainsKey(index))
        {
            items.Remove(index);
            OnItemChanged?.Invoke(index, null);
        }
        
    }
    public void SetItem(Dictionary<int, ItemData> list)
    {
        foreach(var pair in list)
        {
            var instance = pair.Value.CreateInstance();
            if (items.ContainsKey(pair.Key))
            {
                items[pair.Key] = instance;
            }
            else
            {
                items.Add(pair.Key, instance);
            }
            OnItemChanged?.Invoke(pair.Key, instance);
        }
    }

    /// <summary>
    /// ItemData 배열 반환 빈칸은 null 삽입
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, ItemData> GetInventoryData()
    {
        Dictionary<int,ItemData> list = new Dictionary<int, ItemData>();
        foreach(var item in items)
        {
            list.Add(item.Key, item.Value.itemData);
        }
        return list;
    }

    /// <summary>
    /// 인벤토리에 공간이 있는지 확인
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool CanAddItem(out int index)
    {
        for (int i = 0; i < capacity; i++)
        {
            if (items.ContainsKey(i)) continue;
            index = i;
            return true;
        }
        index = -1;
        return false;
    }
}
