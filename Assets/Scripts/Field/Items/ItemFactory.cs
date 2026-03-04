
using System;

public class ItemFactory : Singleton<ItemFactory>
{
    public ItemComponent RequestItem(ItemData data)
    {
        throw new NotImplementedException();
    }
}
