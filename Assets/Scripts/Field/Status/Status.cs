using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    private int _baseValue;
    List<int> modifiers;
    public void Addmodifiers(ref int a)
    {
        modifiers.Add(a);
    }
    public int currentValue
    {
        get
        {
            int value = _baseValue;
            foreach (int modifier in modifiers)
            {
                value += modifier;
            }
            return value;
        }
    }

}