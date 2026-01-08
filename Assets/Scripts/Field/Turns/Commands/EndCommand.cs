using System;
using System.Collections;
using UnityEngine;

public class EndCommand : Command
{

    public override IEnumerator Execute(Action callback = null)
    {
        EditorLogger.Print("EndCommand Execute");
        callback?.Invoke();
        Delete();
        yield break;
    }
}
