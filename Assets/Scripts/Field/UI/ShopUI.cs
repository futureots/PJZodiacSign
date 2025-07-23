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
    private void Awake()
    {
        ToggleUI(false);
    }
    
    void SetShop(List<EntityUIData> entityList, List<ItemData> itemList)
    {
        foreach (EntityUIData entity in entityList)
        {
            
            var instance = Instantiate(entityUI, entityShop);
            Debug.Log(entityUI + " : ");
            var goodsUI = instance.GetComponent<EntityGoodsUI>();
            if(goodsUI != null)
            {
                goodsUI.SetGoods(entity);
            }
            
        }
        foreach (ItemData item in itemList)
        {
            var instance = Instantiate(itemUI, itemShop);
            var goodsUI = instance.GetComponent<ItemGoodsUI>();
            if (goodsUI != null)
            {
                goodsUI.SetGoods(item);
            }
        }
    }
    public void SetPremiumShop()
    {

    }
    public void SetNormalShop()
    {
        var items = table.GetRandomItem(2);
        var entities = table.GetRandomEntity(3);
        SetShop(entities, items);
    }
    public void SetShop(int level)
    {
        if(level %5 == 0)
        {
            SetPremiumShop();
        }
        else
        {
            SetNormalShop();
        }
    }
}
