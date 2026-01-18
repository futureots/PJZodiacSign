using System;
using System.Collections;

public sealed record EnhanceCommand(Entity Target, Entity Resource) : Command
{
    public override IEnumerator Execute(Action callback = null)
    {
        Target.Level += 1;
        Resource.CurTile.ClearOccupant();
        callback?.Invoke();
        Delete();
        yield break;
    }
}
