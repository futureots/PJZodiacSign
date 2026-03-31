using UnityEngine;

public class ItemGoodsUI : GoodsUI<ItemData>
{
    public override void SetGoods(ItemData data)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            var customer = Agent.LocalPlayer;
            if (customer.inventory.items.Count < customer.inventory.capacity)
            {
                var item = ItemFactory.Instance.Request(data);
                customer.inventory.TryAddItem(item);
                customer.Credit -= price;
            }
            else
            {
                Debug.Log("인벤토리에 빈 공간이 없습니다!");
            }
        }));
    }
}
