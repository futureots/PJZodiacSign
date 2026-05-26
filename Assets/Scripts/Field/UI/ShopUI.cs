using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ShopUI : InputManagerUI
{
    private InputManager _inputManager;
    private Agent _agent;
    Shop shop;

    public GameObject shopPanel;
    [SerializeField] GameObject shopToggle;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private EntityGoodsUI entityUI;
    [SerializeField] private ItemGoodsUI itemUI;
    [SerializeField] private Transform entityShop;
    [SerializeField] private Transform itemShop;
    [SerializeField] private GoodsInfoUI goodsInfoUI;
    private List<EntityGoodsUI> _entityGoods;
    private List<ItemGoodsUI> _itemGoods;

    private void Awake()
    {
        _entityGoods = new List<EntityGoodsUI>();
        _itemGoods = new List<ItemGoodsUI>();
    }
    
    public override void Init(InputManager inputManager)
    {
        goodsInfoUI.gameObject.SetActive(false);
        _inputManager = inputManager;
        _agent = _inputManager.agent;
        _agent.OnCreditChanged += UpdateShop;
        UpdateShop(_agent.Credit);
        
        shop = StageManager.Instance.shop;
        shop.OnShopSet += SetShop;
        SetShop();
    }
    
    void SetShop()
    {
        SetShop(shop.entities, shop.items);
    }
    
    void UpdateShop(int credit)
    {
        foreach (EntityGoodsUI item in _entityGoods)
        {
            item.UpdateBuyBtn(credit);
        }
        foreach (ItemGoodsUI item in _itemGoods)
        {
            item.UpdateBuyBtn(credit);
        }
    }
    
    void SetShop(List<EntityData> entityList, List<ItemData> itemList)
    {
        for (int i = 0; i < entityList.Count; i++)
        {
            EntityGoodsUI goodsUI;
            if (_entityGoods.Count > i)
            {
                goodsUI = _entityGoods[i];
            }
            else
            {
                var goods = Instantiate(entityUI, entityShop);
                _entityGoods.Add(goods);
                goodsUI = goods;
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(entityList[i],_inputManager);
                goodsUI.onMouseOver += ShowGoodsInfoUI;
            }
        }
        for (int i = entityList.Count; i < _entityGoods.Count; i++) _entityGoods[i].gameObject.SetActive(false);
        for (int i = 0; i < itemList.Count; i++)
        {
            ItemGoodsUI goodsUI;
            if (_itemGoods.Count > i)
            {
                goodsUI = _itemGoods[i];
            }
            else
            {
                var goods = Instantiate(itemUI, itemShop);
                _itemGoods.Add(goods);
                goodsUI = goods;
            }
            if (goodsUI != null)
            {
                goodsUI.SetGoods(itemList[i],_inputManager);
                goodsUI.onMouseOver += ShowGoodsInfoUI;
            }
        }
        for (int i = itemList.Count; i < _itemGoods.Count; i++) _itemGoods[i].gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 상점 열고 닫기
    /// </summary>
    /// <param name="isOpen"></param>
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

    void ShowGoodsInfoUI(bool isShowed, Vector2 pos,string message)
    {
        if (isShowed)
        {
            goodsInfoUI.gameObject.SetActive(true);
            goodsInfoUI.transform.position = pos;
            goodsInfoUI.SetText(message);
        }
        else
        {
            goodsInfoUI.gameObject.SetActive(false);
        }
    }
    
}
