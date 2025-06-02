using System;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler
{
    public Item item { get; private set; }

    Image image;
    public Image Image
    {
        get
        {
            if(image == null)
            {
                image = transform.GetChild(0).GetComponent<Image>();
            }
            return image;
        }
    }

    public Action<Item> OnClick;
    public void SetSlot(Item item)
    {
        if (item == null) return;
        
        this.item = item;
        Image.sprite = item.itemData.icon;
        Image.color = Color.white;
    }

    public void ClearSlot()
    {
        item = null;
        Image.sprite = null;
        Image.color = Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"Click : {(item != null ? item.itemData.name:null)} ");
        OnClick?.Invoke(item);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //정보 표시 UI 마우스 위치에 표시
    }
}
