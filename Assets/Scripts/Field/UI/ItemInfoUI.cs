using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{
    private ItemComponent _curItem;
    private TextMeshProUGUI _text;

    private TextMeshProUGUI Text
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

    private RectTransform _rectTransform;

    private RectTransform RectTransform
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
        RectTransform.position = vec2;
    }
    public void SetInfo(ItemComponent item)
    {
        if(_curItem != item)
        {
            _curItem = item;
            Text.text = item.ItemData.description;
        }
    }
}
