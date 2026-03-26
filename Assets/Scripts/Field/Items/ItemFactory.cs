using System;


public class ItemFactory : Singleton<ItemFactory>
{
    public ItemComponent RequestItem(ItemData data)
    {
        ItemComponent item = Instantiate(data.prefab);
        item.Init(data);
        return item;
    }
}
