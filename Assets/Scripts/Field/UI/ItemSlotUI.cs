using System;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    int index;
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

    public Action<int> OnClick;
    /// <summary>
    /// 마우스가 들어올 때, 나갈 때 호출되는 함수
    /// </summary>
    public Action<int, Vector2> OnMouseInOut;
    public void SetSlotIndex(int index) => this.index = index;
    /// <summary>
    /// 슬롯에 아이템 설정
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(ItemInstance item)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }
        
        Image.sprite = item.itemData.icon;
        Image.color = Color.white;

    }

    /// <summary>
    /// 슬롯 비우기
    /// </summary>
    public void ClearSlot()
    {
        Image.sprite = null;
        Image.color = Color.clear;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(index);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        // null 입력 시 해당 패널 비활성화
        OnMouseInOut?.Invoke(-1, eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseInOut?.Invoke(index,eventData.position);
    }
}
