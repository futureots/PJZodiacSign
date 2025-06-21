using System.Reflection;
using UnityEngine;

public class ActiveItemInstance : ItemInstance, IActive
{

    public ActiveItemInstance(ItemData itemData) : base(itemData) { }
    public void Activate()
    {
        
    }

    public bool ExecuteSequence()
    {
        return true;
    }

    public bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public void Reinitialize()
    {
        
    }
}
