using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    Button btn;
    
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
    /// <summary>
    /// 마우스가 들어올 때, 나갈 때 호출되는 함수
    /// </summary>
    public event Action<int, Vector2> onMouseMove;
    public event Action<int> onClick;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public void Init(int index)
    {
        this.index = index;
        btn.onClick.AddListener(OnClick);
    }
    /// <summary>
    /// 슬롯에 아이템 설정
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(ItemComponent item)
    {
        if (item == null)
        {
            Image.sprite = null;
            Image.color = Color.clear;
            btn.interactable = false;
            return;
        }
        
        Image.sprite = item.itemData.icon;
        Image.color = Color.white;
        btn.interactable = true;

    }

    void OnClick()
    {
        onClick?.Invoke(index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseMove?.Invoke(-1, eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseMove?.Invoke(index, eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onMouseMove?.Invoke(index, eventData.position);
    }
}
