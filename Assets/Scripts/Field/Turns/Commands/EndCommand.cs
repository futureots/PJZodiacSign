using System.Collections;
using UnityEngine;

public sealed record EndCommand(FieldController Controller) : CheckCommand(Controller)
{
    public override IEnumerator Execute()
    {
        StageManager.Instance.field.RemoveDeadEntities();
        yield return new WaitForSeconds(0.1f);
        EditorLogger.Print("EndCommand Execute");
        if(Controller.CurrentPhase.phaseName == PhaseType.Battle)
        {                
            if (Controller.IsBattleEnd(out var winner))
            {
                Controller.SetPhase();
            }
            else
            {
                Controller.SetTurn();
            }
        }
        else
        {
            Controller.SetTurn();
        }
        Delete();
        yield break;
    }
}
