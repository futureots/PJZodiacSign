using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;


public class ShopUI : InputManagerUI, IPointerClickHandler
{
    private InputManager _inputManager;
    private Agent _agent;
    Shop shop;

    public GameObject shopPanel;
    [SerializeField] Toggle shopToggle;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private EntityGoodsUI entityUI;
    [SerializeField] private ItemGoodsUI itemUI;
    [SerializeField] private Transform entityShop;
    [SerializeField] private Transform itemShop;
    [SerializeField] private GoodsInfoUI goodsInfoUI;
    private List<EntityGoodsUI> _entityGoods;
    private List<ItemGoodsUI> _itemGoods;
    
    [Header("Localization")]
    [SerializeField] private LocalizedString openString;
    [SerializeField] private LocalizedString closeString;

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
        inputManager.agent.fieldController.OnPhaseStarted += PhaseStart;
        
        ToggleButton(true);
        shopToggle.isOn = true;
    }

    void PhaseStart(Phase phase)
    {
        if (phase.phaseName == PhaseType.Battle)
        {
            shopPanel.SetActive(false);
            shopToggle.gameObject.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(true);
            shopToggle.gameObject.SetActive(true);
        }
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
                goodsUI.onMouseClick += ShowGoodsInfoUI;
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
                goodsUI.onMouseClick += ShowGoodsInfoUI;
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
            buttonText.text = closeString.GetLocalizedString();
        }
        else
        {
            _isDescriptionShowed = false;
            _currentDataCache = null;
            goodsInfoUI.gameObject.SetActive(false);
            
            shopPanel.SetActive(false);
            buttonText.text = openString.GetLocalizedString();
        }
    }

    private bool _isDescriptionShowed = false;
    private AbstractData _currentDataCache;

    void ShowGoodsInfoUI(AbstractData data, Vector2 pos)
    {
        if (!_isDescriptionShowed)
        {
            // 비활성화 상태인 경우 활성화
            _isDescriptionShowed = true;
            goodsInfoUI.gameObject.SetActive(true);
            
            // 데이터에 포함된 이동, 공격, 스킬 범위, 스킬 설명관련 세팅
            goodsInfoUI.RectTransform.position = GetProperPos(pos);
            _currentDataCache = data;
            goodsInfoUI.SetInfo(data);
        }
        else if (_currentDataCache != data)
        {
            // 다른 상품을 선택하면 활성화 상태 유지, 대신 선택한 상품의 정보로 변경
            goodsInfoUI.RectTransform.position = GetProperPos(pos);
            _currentDataCache = data;
            goodsInfoUI.SetInfo(data);
        }
        else
        {
            _currentDataCache = null;
            _isDescriptionShowed = false;
            goodsInfoUI.gameObject.SetActive(false);
            
        }
    }

    Vector2 GetProperPos(Vector2 pos)
    {
        float uiWidth = goodsInfoUI.RectTransform.rect.width;
        float uiHeight = goodsInfoUI.RectTransform.rect.height;
        var targetPos = pos;
        // 화면 우측 이탈 방지
        if (targetPos.x + uiWidth > Screen.width)
        {
            targetPos.x -= uiWidth;
        }

        // 화면 하단 이탈 방지
        if (targetPos.y - uiHeight < 0)
        {
            targetPos.y += uiHeight;
        }

        return targetPos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _currentDataCache = null;
        _isDescriptionShowed = false;
        goodsInfoUI.gameObject.SetActive(false);
    }
}
