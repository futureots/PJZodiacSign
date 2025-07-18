using System.Reflection;
using UnityEngine;

public class ActiveItemInstance : ItemInstance, IUsable
{
    public IActive effect;
    public ActiveItemInstance(ItemData itemData) : base(itemData) { }

    public IActive GetUseEffect()
    {
        return effect;
    }
}