using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 현재 에이전트가 보유하고 있는 아이템
    public List<ItemInstance> items { get; private set; }
    public Action<int, ItemInstance> OnItemChanged;
    public int capacity;
    private void Awake()
    {
        items = new List<ItemInstance>();
    }
    public bool AddItem(ItemInstance instance)
    {
        // 중간에 빈공간 먼저 삽입
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i] == null)
            {
                items[i] = instance;
                OnItemChanged?.Invoke(i, instance);
                return true;
            }
        }
        if(items.Count < capacity)
        {
            int lastIndex = items.Count;
            items.Add(instance);
            OnItemChanged?.Invoke(lastIndex, instance);
            return true;
        }
        return false;
    }
    public bool AddItem(ItemData data)
    {
        return AddItem(data.CreateInstance());
    }

    public void RemoveItem(int index)
    {
        items[index] = null;
        OnItemChanged?.Invoke(index, null);
    }
    public void SetItem(List<ItemData> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == null) continue;
            var instance = list[i].CreateInstance();
            if (items.Count > i)
            {
                items[i] = instance;
                OnItemChanged?.Invoke(i, instance);
            }
            else
            {
                items.Add(instance);
                OnItemChanged?.Invoke(i, instance);
            }
        }
    }

    /// <summary>
    /// ItemData 배열 반환 빈칸은 null 삽입
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetInventoryData()
    {
        List<ItemData> list = new List<ItemData>();
        foreach(var item in items)
        {
            if (item != null)
            {
                list.Add(item.itemData);
            }
            else list.Add(null);
        }
        return list;
    }
}
