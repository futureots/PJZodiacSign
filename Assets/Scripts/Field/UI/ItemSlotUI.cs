using System;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    public ItemInstance item { get; private set; }

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
    /// <summary>
    /// 마우스가 들어올 때, 나갈 때 호출되는 함수
    /// </summary>
    public Action<ItemInstance, Vector2> OnMouseInOut;
    public void SetSlot(ItemInstance item)
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

    public void UseSlot()
    {
        // 사용 등록(바로 삭제 X, 해당 명령 실행 시 삭제)
        if(item is ActiveItemInstance)
        {
            var active = (ActiveItemInstance)item;
            active.effect.AddCallback( x=>
            {
                if (x) ClearSlot();
            });
            transform.root.GetComponent<InputManager>().SetInputMode(active.effect);
        }
        else
        {
            Debug.Log("Not Active Item");
        }
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
        OnMouseInOut?.Invoke(null, eventData.position);
        //Debug.Log("Out");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            OnMouseInOut?.Invoke(item, eventData.position);
            //Debug.Log("enter");
        }
    }
}
