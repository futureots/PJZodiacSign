using System.Collections;
using UnityEngine;


public record CheckCommand(FieldController Controller) :  Command
{
    public override IEnumerator Execute()
    {
        StageManager.Instance.field.RemoveDeadEntities();
        yield return new WaitForSeconds(0.1f);
        if(Controller.CurrentPhase.phaseName == PhaseType.Battle)
        {
            if (Controller.IsBattleEnd(out var winner))
            {
                Controller.SetPhase();
            }
        }
        Delete();
        yield break;
    }
}
