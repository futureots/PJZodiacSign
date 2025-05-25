using UnityEngine;

[System.Serializable]
public class Item
{
    public readonly ItemData itemData;

    public Item(ItemData itemData)
    {
        this.itemData = itemData;
    }

}
