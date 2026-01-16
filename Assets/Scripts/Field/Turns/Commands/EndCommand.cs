using System;
using System.Collections;

public sealed record EndCommand(FieldController Controller) : Command
{
    public override IEnumerator Execute(Action callback = null)
    {
        EditorLogger.Print("EndCommand Execute");
        if(Controller.CurrentPhase.phaseName == PhaseType.Battle)
        {                // TODO : 페이즈의 종료조건 확인, 함수 따로 만들어서 확인
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
        callback?.Invoke();
        Delete();
        yield break;
    }
}
