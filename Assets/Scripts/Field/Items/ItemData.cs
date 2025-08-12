using System;
using UnityEngine;

public class ItemData : AbstractData
{
    public string description;

    public virtual ItemInstance CreateInstance()
    {
        return new ItemInstance(this);
    }
    public UseType type;

}
[Flags]
public enum UseType
{
    None = 0,
    Battle = 1 << 0,
    Repair = 1 << 1
}