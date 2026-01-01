using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{
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
    // Update is called once per frame
    void Update()
    {
        RectTransform.anchoredPosition = Input.mousePosition;
    }
    public void SetInfo(ItemComponent item)
    {
        Text.text = item.itemData.description;
    }
}
