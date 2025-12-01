using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ShopUI : MonoBehaviour
{
    [Header("Dependency")]
    InputManager customer;
    IPhaseManageService phaseManageService;     // TODO: phase 의존성 주입

    public GameObject shopPanel;
    [SerializeField] GameObject ShopToggle;

    [SerializeField] EntityGoodsUI entityUI;
    [SerializeField] ItemGoodsUI itemUI;
    [SerializeField] Transform entityShop;
    [SerializeField] Transform itemShop;

    [SerializeField] ShopTable table;

    List<ItemData> items;
    List<EntityData> entities;

    public void Init(InputManager inputManager,IPhaseManageService phaseManageService)
    {
        this.phaseManageService = phaseManageService;
        customer = inputManager;
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
        entities = table.GetRandomEntity(8);
        SetShop(entities, items);
    }
    void SetNormalShop()
    {
        items = table.GetRandomItem(2);
        entities = table.GetRandomEntity(3);
        SetShop(entities, items);
    }
    void SetShop(List<EntityData> entityList, List<ItemData> itemList)
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
                goodsUI.SetGoods(entityList[i],customer);
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
                goodsUI.SetGoods(itemList[i],customer);
            }
        }
        for (int i = itemList.Count; i < itemShop.childCount; i++) itemShop.GetChild(i).gameObject.SetActive(false);
    }
    void PhaseChange(IPhase curPhase, bool start)
    {
        if (curPhase is RepairPhase phase)
        {
            // 페이즈 시작
            if (start)
            {
                ShopToggle.SetActive(true);
                SetShop(phase.Level);
            }
            // 페이즈 종료
            else
            {
                shopPanel.SetActive(false);
                ShopToggle.SetActive(false);
            }
        }
    }
}
