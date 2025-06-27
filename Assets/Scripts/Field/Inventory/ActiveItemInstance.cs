using System.Reflection;
using UnityEngine;

public class ActiveItemInstance : ItemInstance
{
    public IActive effect;
    public ActiveItemInstance(ItemData itemData) : base(itemData) { }


}
