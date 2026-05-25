using UnityEngine;

public class ItemGoodsUI : GoodsUI<ItemData>
{
    public override void SetGoods(ItemData data,InputManager inputManager)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var customer = inputManager.agent;
            var item = ItemFactory.Instance.Request(data);
            if (customer.inventory.TryAddItem(item))
            {
                customer.Credit -= price;
                EditorLogger.Print($"buy item : {item.ItemData.id}");
            }
            else
            {
                inputManager.onMessageActivated.Invoke("인벤토리에 빈 공간이 없습니다!",LogType.Error);
            }
        });
    }
}
