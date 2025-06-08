using System;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
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

    public Action<ItemSlotUI> OnClick;
    public Action<Item, Vector2> OnMouseOver;
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
        if(item != null)
        {
            OnClick?.Invoke(this);
        }
        
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        // null 입력 시 해당 패널 비활성화
        OnMouseOver?.Invoke(null, eventData.position);
        Debug.Log("Out");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            OnMouseOver?.Invoke(item, eventData.position);
            Debug.Log("enter");
        }
    }
}
