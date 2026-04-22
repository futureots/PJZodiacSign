using UnityEngine;

public class ItemGoodsUI : GoodsUI<ItemData>
{
    public override void SetGoods(ItemData data,LogSystem log = null)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var customer = Agent.LocalPlayer;
            var item = ItemFactory.Instance.Request(data);
            if (customer.inventory.TryAddItem(item))
            {
                customer.Credit -= price;
            }
            else
            {
                log?.ShowLog("인벤토리에 빈 공간이 없습니다!",LogType.Error);
                EditorLogger.Print("인벤토리에 빈 공간이 없습니다!");
            }
        });
    }
}
