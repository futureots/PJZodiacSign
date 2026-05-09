using System;
using System.Collections;

public sealed record AttackCommand(Entity Target, int Multiplier = 100) : Command
{
    public override IEnumerator Execute()
    {
        yield return Target.StartCoroutine(Target.Attack(Multiplier));
        Delete();
    }

}
