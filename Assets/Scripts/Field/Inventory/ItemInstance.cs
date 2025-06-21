using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData itemData { get; private set; }
    public int count;
    public ItemInstance(ItemData itemData)
    {
        this.itemData = itemData;
    }


    



}
