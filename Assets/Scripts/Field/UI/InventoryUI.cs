using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public bool isOpen { get; private set; } = false;
    [ContextMenuItem("SetClosePos", "SetClosedPosition")]
    public Vector3 closedPosition;
    [ContextMenuItem("SetOpenPos", "SetOpenedPosition")]
    public Vector3 openedPosition;
    #region Debugging
    public void SetClosedPosition()
    {
        closedPosition = transform.localPosition;
    }
    public void SetOpenedPosition()
    {
        openedPosition = transform.localPosition;
    }
    #endregion
    public Button popBtn;

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    public void PopInventory()
    {
        isOpen = !isOpen;
        var panel = GetComponent<RectTransform>();
        if (isOpen)
        {
            popBtn.transform.localScale = new Vector3(1, 1, 1);
            panel.DOLocalMove(openedPosition, 0.5f);
        }
        else
        {
            popBtn.transform.localScale = new Vector3(-1, 1, 1);
            panel.DOLocalMove(closedPosition, 0.5f);
        }
    }


}
