using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ShopUI : MonoBehaviour
{
    [SerializeField] EntityGoodsUI entityUI;
    [SerializeField] ItemGoodsUI itemUI;
    [SerializeField] Transform entityShop;
    [SerializeField] Transform itemShop;

    [SerializeField] ShopTable table;
    
    bool isOpen;
    public void ToggleUI()
    {
        isOpen = !isOpen;
        ToggleUI(isOpen);
    }
    public void ToggleUI(bool open)
    {
        if (open)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    void SetShop(List<EntityUIData> entityList, List<ItemData> itemList)
    {
        for (int i = 0; i < entityList.Count; i++)
        {
            EntityGoodsUI goodsUI;
            if (entityShop.childCount > i)
            {
                goodsUI = entityShop.GetChild(i).GetComponent<EntityGoodsUI>();
            }
            else
            {
                goodsUI = Instantiate(entityUI, entityShop).GetComponent<EntityGoodsUI>();
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(entityList[i]);
            }
        }
        for (int i = entityList.Count; i < entityShop.childCount; i++) entityShop.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < itemList.Count; i++)
        {
            ItemGoodsUI goodsUI;
            if (itemShop.childCount > i)
            {
                goodsUI = itemShop.GetChild(i).GetComponent<ItemGoodsUI>();
            }
            else
            {
                goodsUI = Instantiate(itemUI, itemShop).GetComponent<ItemGoodsUI>();
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(itemList[i]);
            }
        }
        for (int i = itemList.Count; i < itemShop.childCount; i++) itemShop.GetChild(i).gameObject.SetActive(false);
    }
    void SetPremiumShop()
    {

    }
    void SetNormalShop()
    {
        var items = table.GetRandomItem(2);
        var entities = table.GetRandomEntity(3);
        SetShop(entities, items);
    }
    public void SetShop(int level, bool isPremium = false)
    {
        if(level %5 == 0 || isPremium)
        {
            SetPremiumShop();
        }
        else
        {
            SetNormalShop();
        }
    }
}
