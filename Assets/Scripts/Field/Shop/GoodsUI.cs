using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GoodsUI<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler where T : AbstractData 
{
    [SerializeField] protected T data;
    [SerializeField] protected Button buyBtn;
    [SerializeField] protected TextMeshProUGUI goodsName;
    [SerializeField] protected TextMeshProUGUI priceText;
    [SerializeField] protected Image icon;
    protected int price;

    /// <summary>
    /// 해당 UI가 표시할 오브젝트 세팅
    /// </summary>
    /// <param name="data"></param>
    /// <param name="inputManager"></param>
    public virtual void SetGoods(T data, InputManager inputManager = null)
    {
        goodsName.text = data.productName;
        price = Mathf.RoundToInt(data.normalPrice * Random.Range(0.8f, 1.2f));
        priceText.text = price.ToString();
        icon.sprite = data.icon;
    }

    /// <summary>
    /// 소비자가 상품을 구매할 크레딧을 보유하고 있는지 확인
    /// </summary>
    /// <param name="credit"></param>
    public void UpdateBuyBtn(int credit)
    {
        if (credit < price)
        {
            buyBtn.interactable = false;
        }
        else
        {
            buyBtn.interactable = true;
        }
    }

    private void OnEnable()
    {
        //customer.onCreditChanged += UpdateBuyBtn;
    }
    private void OnDisable()
    {
        //customer.onCreditChanged -= UpdateBuyBtn;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EditorLogger.Print("OnPointerEnter");
        // 설명 UI 활성화
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EditorLogger.Print("OnPointerExit");
        // 설명 UI 비활성화
    }
}
