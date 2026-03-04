using System;
using System.Collections;

public sealed record EnhanceCommand(Entity Target, Entity Resource) : Command
{
    public override IEnumerator Execute()
    {
        Target.Level += 1;
        Resource.CurTile.ClearOccupant();
        Delete();
        yield break;
    }
}
