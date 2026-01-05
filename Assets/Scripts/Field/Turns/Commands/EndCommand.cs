using System;
using System.Collections;
using UnityEngine;

public class EndCommand : Command
{
    public EndCommand() 
    {
        selecterObjects = new();
    }

    public override IEnumerator Execute(Action callback = null)
    {
        Debug.Log("EndCommand Execute");
        return base.Execute(callback);
    }
}
