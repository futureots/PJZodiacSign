using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGoodsUI : GoodsUI<ItemData>
{



    public override void SetGoods(ItemData data)
    {
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var agent = transform.root.GetComponent<Agent>();
            agent.BuyItem(data,price);
        });
    }
}
