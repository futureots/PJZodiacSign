using System;
using UnityEngine;


public class ItemFactory : Singleton<ItemFactory>
{
    
    
    /// <summary>
    /// Create Item Object
    /// </summary>
    /// <param name="data">Item Data : SO</param>
    /// <returns>Item Component</returns>
    public ItemComponent Request(ItemData data)
    {
        ItemComponent item = Instantiate(data.prefab);
        item.Init(data);
        
        return item;
    }
}
