using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{
    ItemComponent curItem;
    TextMeshProUGUI _text;
    public TextMeshProUGUI Text
    {
        get
        {
            if( _text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
            }
            return _text;
        }
    }
    RectTransform _rectTransform;
    RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    public void SetPosition(Vector2 vec2)
    {
        RectTransform.anchoredPosition = vec2;
    }
    public void SetInfo(ItemComponent item)
    {
        if(curItem != item)
        {
            curItem = item;
            Text.text = item.itemData.description;
        }
    }
}
