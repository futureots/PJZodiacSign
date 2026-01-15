
using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ShopTable table;

    public List<EntityData> entities;
    public List<ItemData> items;

    public Action OnShopSet;
    public void Init( ShopTable table)
    {
        this.table = table;
        SetShop(GameManager.Instance.Level);
        OnShopSet?.Invoke();
    }

    /// <summary>
    /// 현재 레벨에 맞는 상점 세팅
    /// </summary>
    /// <param name="level"></param>
    /// <param name="isPremium"></param>
    public void SetShop(int level, bool isPremium = false)
    {
        if (level % 5 == 0 || isPremium)
        {
            SetPremiumShop();
        }
        else
        {
            SetNormalShop();
        }
    }
    void SetPremiumShop()
    {
        items = table.GetRandomItem(5);
        entities = table.GetRandomEntity(5);
    }
    void SetNormalShop()
    {
        items = table.GetRandomItem(2);
        entities = table.GetRandomEntity(3);
    }
}
