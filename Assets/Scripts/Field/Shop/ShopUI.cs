
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ShopUI : MonoBehaviour
{
    Agent agent;
    Shop shop;

    public GameObject shopPanel;
    [SerializeField] GameObject shopToggle;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] EntityGoodsUI entityUI;
    [SerializeField] ItemGoodsUI itemUI;
    [SerializeField] Transform entityShop;
    [SerializeField] Transform itemShop;
    List<EntityGoodsUI> entityGoods;
    List<ItemGoodsUI> itemGoods;

    private void Awake()
    {
        entityGoods = new List<EntityGoodsUI>();
        itemGoods = new List<ItemGoodsUI>();
        shop = StageManager.Instance.shop;
        shop.OnShopSet += SetShop;
        Agent.OnLocalPlayerChanged += SetAgent;
    }
    private void OnDestroy()
    {
        Agent.OnLocalPlayerChanged -= SetAgent;
    }

    void SetShop()
    {
        SetShop(shop.entities, shop.items);
    }
    void SetAgent()
    {
        if (agent)
        {
            agent.OnCreditChanged -= UpdateShop;
        }
        agent = Agent.LocalPlayer;
        agent.OnCreditChanged += UpdateShop;
        UpdateShop(agent.Credit);
    }
    void UpdateShop(int credit)
    {
        foreach (EntityGoodsUI item in entityGoods)
        {
            item.UpdateBuyBtn(credit);
        }
        foreach (ItemGoodsUI item in itemGoods)
        {
            item.UpdateBuyBtn(credit);
        }
    }
    void SetShop(List<EntityData> entityList, List<ItemData> itemList)
    {
        for (int i = 0; i < entityList.Count; i++)
        {
            EntityGoodsUI goodsUI;
            if (entityGoods.Count > i)
            {
                goodsUI = entityGoods[i];
            }
            else
            {
                var goods = Instantiate(entityUI, entityShop);
                entityGoods.Add(goods);
                goodsUI = goods;
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(entityList[i]);
            }
        }
        for (int i = entityList.Count; i < entityGoods.Count; i++) entityGoods[i].gameObject.SetActive(false);
        for (int i = 0; i < itemList.Count; i++)
        {
            ItemGoodsUI goodsUI;
            if (itemGoods.Count > i)
            {
                goodsUI = itemGoods[i];
            }
            else
            {
                var goods = Instantiate(itemUI, itemShop);
                itemGoods.Add(goods);
                goodsUI = goods;
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(itemList[i]);
            }
        }
        for (int i = itemList.Count; i < itemGoods.Count; i++) itemGoods[i].gameObject.SetActive(false);
    }

    public void ToggleButton(bool isOpen)
    {
        if (isOpen)
        {
            shopPanel.SetActive(true);
            buttonText.text = "닫기";
        }
        else
        {
            shopPanel.SetActive(false);
            buttonText.text = "상점";
        }
    }
}
