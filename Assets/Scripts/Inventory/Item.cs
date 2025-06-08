using UnityEngine;

[System.Serializable]
public class Item
{
    public readonly ItemData itemData;
    public int count;
    public Item(ItemData itemData)
    {
        this.itemData = itemData;

    }

}
