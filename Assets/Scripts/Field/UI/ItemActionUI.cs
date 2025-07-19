using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemActionUI : MonoBehaviour
{
    public Button useBtn;
    public Button discardBtn;

    public void SetItemAction(Inventory inventory, int index)
    {
        discardBtn.onClick.RemoveAllListeners();
        discardBtn.onClick.AddListener(() => inventory.RemoveItem(index));
        discardBtn.onClick.AddListener(() => gameObject.SetActive(false));
        
        if(inventory.items[index] is IUsable usable)
        {
            var effect = usable.GetUseEffect();
            useBtn.gameObject.SetActive(true);

            useBtn.onClick.RemoveAllListeners();
            useBtn.onClick.AddListener(()=>
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
            useBtn.gameObject.SetActive(false);
        }

    }



}
