using Battle.Phase;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    Inventory inventory;
    ItemComponent curItem;
    public Button useBtn;
    public Button discardBtn;
    public Action<ItemComponent> onUseItem;
    public void Init(Inventory _inventory)
    {
        inventory = _inventory;
        discardBtn.onClick.AddListener(() => {
            inventory.RemoveItem(curItem);
            gameObject.SetActive(false);
            curItem = null;
            });
        useBtn.onClick.AddListener(() =>
        {
            onUseItem?.Invoke(curItem);
            gameObject.SetActive(false);
            curItem = null;
        });

    }

    /// <summary>
    /// 각 버튼의 상호작용 설정
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="index"></param>
    public void SetItemAction(ItemComponent item, TurnType curTurn)
    {
        curItem = item;
        useBtn.gameObject.SetActive(true);

        if (item.itemData.useType.HasFlag(curTurn))
        {
            useBtn.interactable = true;
        }
        else
        {
            useBtn.interactable = false;
        }
    }
}
