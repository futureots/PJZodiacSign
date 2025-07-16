using System.Diagnostics;
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

    private void Start()
    {
        SetGoods(data);
    }

    public void SetGoods(ItemData data)
    {
        goodsName.text = data.name;
        price.text = data.cost.ToString();
        icon.sprite = data.icon;
        buyBtn.onClick.AddListener(() =>
        {
            if (DataManager.Instance.playerData.credit >= data.cost)
            {
                DataManager.Instance.playerData.credit -= data.cost;
                InputManager.Instance.UI.inventory.AddItem(data.CreateInstance());
            }

        });
    }
}
