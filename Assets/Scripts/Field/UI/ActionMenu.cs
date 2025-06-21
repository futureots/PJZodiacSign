using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public Button useBtn;
    public Button discardBtn;

    public void SetItemAction(ItemSlotUI slot)
    {
        discardBtn.onClick.RemoveAllListeners();
        discardBtn.onClick.AddListener(slot.ClearSlot);
        discardBtn.onClick.AddListener(() => gameObject.SetActive(false));
        
        if(slot.item is ActiveItemInstance)
        {
            useBtn.gameObject.SetActive(true);
            Debug.Log("is Active");

            useBtn.onClick.RemoveAllListeners();
            useBtn.onClick.AddListener(slot.UseSlot);
            useBtn.onClick.AddListener(() => gameObject.SetActive(false));
        }
        else
        {
            useBtn.gameObject.SetActive(false);
        }

    }
}
