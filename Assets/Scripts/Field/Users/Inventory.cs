using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 유저가 들고 있는 아이템
    /// </summary>
    public ItemComponent[] items { get; private set; }
    public Action<int, ItemComponent> OnItemChanged;
    public int capacity;
    private void Awake()
    {
        items = new ItemComponent[capacity];
    }
    private void Start()
    {
    }

    public bool AddItem(ItemComponent instance)
    {
        // 용량 개수 만큼 확인 및 빈 공간에 추가
        if (CanAddItem(out int index))
        {
            items[index] = instance;
            OnItemChanged?.Invoke(index, instance);
            return true;
        }
        
        // 용량 부족
        return false;
    }
    /// <summary>
    /// 인벤토리 아이템 제거
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        if(items[index] != null)
        {
            items[index] = null;
            OnItemChanged?.Invoke(index, null);
        }
        
    }
    public void SetItem(List<ItemData> list)
    {
        // TODO : 풀링된 아이템 또는 직접 아이템 오브젝트를 불러와 인벤토리에 세팅
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null) continue;
            
            //var instance = list[i].CreateInstance();
            //items[i] = instance;
            //OnItemChanged?.Invoke(i, instance);
        }
    }

    /// <summary>
    /// ItemData 배열 반환 빈칸은 null 삽입
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetInventoryData()
    {
        List<ItemData> list = new(capacity);
        for(int i = 0; i < items.Length; i++)
        {
            if( items[i] == null) continue;
            list[i] = items[i].itemData;
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
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null) continue;
            index = i;
            return true;
        }
        index = -1;
        return false;
    }
}
