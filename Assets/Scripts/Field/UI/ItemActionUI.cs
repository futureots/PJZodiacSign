using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemActionUI : MonoBehaviour
{
    public Button useBtn;
    public Button discardBtn;

    /// <summary>
    /// 각 버튼의 상호작용 설정
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="index"></param>
    public void SetItemAction(Inventory inventory, int index)
    {
        discardBtn.onClick.RemoveAllListeners();
        discardBtn.onClick.AddListener(() => inventory.RemoveItem(index));
        discardBtn.onClick.AddListener(() => gameObject.SetActive(false));

        var item = inventory.items[index];
        if (item is IUsable usable)
        {
            var effect = usable.GetUseEffect();
            useBtn.gameObject.SetActive(true);

            if (item.itemData.useType.HasFlag(PhaseManager.curPhase))
            {
                useBtn.interactable = true;

                useBtn.onClick.RemoveAllListeners();
                useBtn.onClick.AddListener(() =>
                {
                    effect.AddCallback(x =>
                    {
                        if (x) inventory.RemoveItem(index);
                        effect.ClearCallback();
                    });
                    transform.root.GetComponent<InputManager>().SetInputMode(effect);
                });
                useBtn.onClick.AddListener(() => gameObject.SetActive(false));
            }
            else
            {
                useBtn.interactable = false;
            }
        }
        else
        {
            useBtn.gameObject.SetActive(false);
        }

    }



}
