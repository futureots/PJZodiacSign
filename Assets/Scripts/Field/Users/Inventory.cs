using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 유저가 들고 있는 아이템
    /// </summary>
    public List<ItemComponent> items { get; private set; }
    public Action<int, ItemComponent> OnItemChanged;
    public int capacity;
    private void Awake()
    {
        items = new List<ItemComponent>();
    }

    /// <summary>
    /// 생성된 아이템 오브젝트를 인벤토리에 추가
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool TryAddItem(ItemComponent instance)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i] == null)
            {
                items[i] = instance;
                OnItemChanged(i, instance);
                instance.transform.SetParent(transform);
                return true;
            }
        }
        // 용량 초과
        if (items.Count >= capacity)
        {
            return false;
        }
        items.Add(instance);
        OnItemChanged(items.Count-1, instance);
        instance.transform.SetParent(transform);
        instance.OnDiscard += () => RemoveItem(instance);
        return true;

    }
    /// <summary>
    /// 인벤토리 아이템 제거
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(ItemComponent item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                OnItemChanged(i, null);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void SetItem(List<ItemData> list)
    {
        items.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                items.Add(null);
                OnItemChanged?.Invoke(i, null);
                continue;
            }
            var item = ItemFactory.Instance.Request(list[i]);
            items.Add(item);
            OnItemChanged?.Invoke(i, item);
            item.transform.SetParent(transform);
            item.OnDiscard += () => RemoveItem(item);
        }
    }

    /// <summary>
    /// ItemData 배열 반환 빈칸은 null 삽입
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetInventoryData()
    {
        List<ItemData> list = new List<ItemData>();
        for(int i = 0; i < items.Count; i++)
        {
            if (!items[i]) list.Add(null);
            else
            {
                list.Add(items[i].itemData);
            }
                
        }
        return list;
    }
}
