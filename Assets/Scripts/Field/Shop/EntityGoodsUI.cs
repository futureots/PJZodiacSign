using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityGoodsUI : GoodsUI<EntityData>
{
    public override void SetGoods(EntityData data)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var agent = transform.root.GetComponent<Agent>();
            agent.BuyEntity(data,price);

            
        });
    }
}
