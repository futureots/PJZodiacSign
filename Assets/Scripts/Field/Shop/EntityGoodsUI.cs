using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityGoodsUI : GoodsUI<EntityData>
{
    public override void SetGoods(EntityData data, Agent customer)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data, customer);
        buyBtn.onClick.AddListener(() =>
        {
            if (customer.SummonEntity(data))
            {
                customer.Credit -= price;
            }
            else
            {
                Debug.Log("소환할 빈 공간이 없습니다!");
            }
        });
    }
}
