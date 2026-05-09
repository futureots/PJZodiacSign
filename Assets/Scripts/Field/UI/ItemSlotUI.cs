using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Button button;

    private int _index;
    [SerializeField] private Image image;
    /// <summary>
    /// 마우스가 들어올 때, 나갈 때 호출되는 함수
    /// </summary>
    public event Action<int, Vector2> OnMouseMove;
    public event Action<int> OnClick;
    
    public void Init(int index)
    {
        _index = index;
        button.onClick.AddListener(Click);
    }
    /// <summary>
    /// 슬롯에 아이템 설정
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(ItemComponent item)
    {
        if (!item)
        {
            image.sprite = null;
            image.color = Color.clear;
            //button.interactable = false;
            return;
        }
        
        image.sprite = item.ItemData.icon;
        image.color = Color.white;
        //button.interactable = true;

        item.onDiscard += () => SetSlot(null);
    }

    void Click()
    {
        OnClick?.Invoke(_index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseMove?.Invoke(-1, transform.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseMove?.Invoke(_index, transform.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //OnMouseMove?.Invoke(_index, eventData.position);
    }
}
