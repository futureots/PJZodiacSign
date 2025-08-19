using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 현재 에이전트가 보유하고 있는 아이템
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
        for(int i = 0; i < capacity; i++)
        {
            if (items.ContainsKey(i)) continue;
            items.Add(i, instance);
            OnItemChanged?.Invoke(i, instance);
            return true;
        }
        // 용량 부족
        return false;
    }
    public bool AddItem(ItemData data)
    {
        return AddItem(data.CreateInstance());
    }

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
}
