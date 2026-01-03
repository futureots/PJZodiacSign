using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGoodsUI : GoodsUI<ItemData>
{
    public override void SetGoods(ItemData data, Agent customer)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data, customer);
        buyBtn.onClick.AddListener(() =>
        {
            // TODO : 인벤토리에 구매한 아이템 추가
            //if (customer.inventory.AddItem(data))
            //{
            //    customer.Credit -= price;
            //}
            //else
            //{
            //    Debug.Log("인벤토리에 빈 공간이 없습니다!");
            //}
        });
    }
}
