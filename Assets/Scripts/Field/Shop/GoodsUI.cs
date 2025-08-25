using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsUI<T> : MonoBehaviour where T : AbstractData
{
    [SerializeField] protected T data;
    protected Agent customer;
    [SerializeField] protected Button buyBtn;
    [SerializeField] protected TextMeshProUGUI goodsName;
    [SerializeField] protected TextMeshProUGUI priceText;
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI countText;
    protected int price;

    public virtual void SetGoods(T data, Agent customer)
    {
        this.customer = customer;
        goodsName.text = data.productName;
        price = data.normalPrice;
        priceText.text = price.ToString();
        icon.sprite = data.icon;
        UpdateBuyBtn(customer.Credit);
    }
    void UpdateBuyBtn(int credit)
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
        customer.onCreditChanged += UpdateBuyBtn;
    }
    private void OnDisable()
    {
        customer.onCreditChanged -= UpdateBuyBtn;
    }
}
