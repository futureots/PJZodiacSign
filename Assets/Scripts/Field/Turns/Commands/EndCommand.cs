using System;
using System.Collections;

public sealed record EndCommand(FieldController Controller) : Command
{
    public override IEnumerator Execute(Action callback = null)
    {
        EditorLogger.Print("EndCommand Execute");
        
        // FieldController.CheckBattleEnd()
        // if true : fieldcontroller.EndBattle()
        // else : SetTurn()
        callback?.Invoke();
        Delete();
        yield break;
    }
}
