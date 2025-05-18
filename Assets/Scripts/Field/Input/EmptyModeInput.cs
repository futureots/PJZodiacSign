using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyModeInput : IModeInput
{
    public void RemoveMode()
    {
        
    }

    public void SetMode()
    {
        Debug.Log("SetEmptyMode");
    }
}
