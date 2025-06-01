using DG.Tweening;
using NUnit.Framework;
using System;
using System.Reflection;
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
    
    private void Start()
    {
        PopInventory(false);
    }
    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    public void PopInventory()
    {
        isOpen = !isOpen;
        PopInventory(isOpen);
    }

    void PopInventory(bool isOpen)
    {
        var panel = GetComponent<RectTransform>();
        var pos = isOpen ? openedPosition : closedPosition;
        var scaleX = isOpen ? 1 : -1;
        popBtn.transform.localScale = new Vector3(scaleX, 1, 1);
        panel.DOLocalMove(pos, 0.5f);
    }
}
