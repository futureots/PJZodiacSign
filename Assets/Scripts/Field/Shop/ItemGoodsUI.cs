using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGoodsUI : MonoBehaviour
{
    [SerializeField] ItemData data;

    [SerializeField] Button buyBtn;
    [SerializeField] TextMeshProUGUI goodsName;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] protected Image icon;


    public void SetGoods(ItemData data)
    {
        goodsName.text = data.itemName;
        price.text = data.cost.ToString();
        icon.sprite = data.icon;
        buyBtn.onClick.AddListener(() =>
        {
            var agent = transform.root.GetComponent<Agent>();
            agent.BuyItem(data);
        });
    }
}
