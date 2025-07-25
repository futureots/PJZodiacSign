using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsUI<T> : MonoBehaviour where T : AbstractData
{
    [SerializeField] protected T data;

    [SerializeField] protected Button buyBtn;
    [SerializeField] protected TextMeshProUGUI goodsName;
    [SerializeField] protected TextMeshProUGUI priceText;
    [SerializeField] protected Image icon;
    protected int price;

    public virtual void SetGoods(T data)
    {
        goodsName.text = data.productName;
        price = data.normalPrice;
        priceText.text = price.ToString();
        icon.sprite = data.icon;
    }
}
