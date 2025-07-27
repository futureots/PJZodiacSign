using System;
using UnityEngine;


public class ItemData : AbstractData
{
    public string description;

    public virtual ItemInstance CreateInstance()
    {
        return new ItemInstance(this);
    }
}
