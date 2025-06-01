using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Item item;
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
}
