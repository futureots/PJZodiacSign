using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    [Header("Dependency")]
    IPhaseManageService phaseManageService;

    public Button useBtn;
    public Button discardBtn;

    public void Init(IPhaseManageService phaseManageService)
    {
        this.phaseManageService = phaseManageService;
    }

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

            if (item.itemData.useType.HasFlag(phaseManageService.CurPhase))
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
