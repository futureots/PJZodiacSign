
using GlobalManage;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ShopTable table;

    public List<EntityData> entities;
    public List<ItemData> items;

    public Action OnShopSet;
    public void Init(ShopTable table)
    {
        this.table = table;
        SetShop(GameManager.Instance.Level);        // TODO: Level 관련 GameManager 의존성 제거
        OnShopSet?.Invoke();
    }

    /// <summary>
    /// 현재 레벨에 맞는 상점 세팅
    /// </summary>
    /// <param name="level"></param>
    /// <param name="isPremium"></param>
    private void SetShop(int level, bool isPremium = false)
    {
        if (level <= 1)
        {
            //items = table.GetRandomItem(3);
            entities = table.GetRandomEntity(5,ShopTable.ShopType.Normal);
        }
        else if ((level % 4 == 1) || isPremium)
        {
            items = table.GetRandomItem(5);
            var list = table.GetRandomEntity(2, ShopTable.ShopType.Premium);
            list.AddRange(table.GetRandomEntity(3,ShopTable.ShopType.Normal));
            entities = list;
        }
        else
        {
            items = table.GetRandomItem(2);
            entities = table.GetRandomEntity(3, ShopTable.ShopType.Normal);
        }
    }

    /// <summary>
    /// 크레딧으로 살 수 있는 기물 중 랜덤 1개를 택해서 반환한다.
    /// </summary>
    /// <param name="credit"></param>
    /// <param name="shopType">상점 타입</param>
    /// <returns></returns>
    public EntityData GetRandomEntity(int credit, ShopTable.ShopType shopType)
    {
        if (table.TryGetBuyableEntity(credit, out EntityData entityData,shopType))
        {
            return entityData;
        }
        else
        {
            return null;
        }
        
    }
}
