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
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
    public void SetInfo(Item item)
    {
        Text.text = item.itemData.description;
    }
}
