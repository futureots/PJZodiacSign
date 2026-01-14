using System;
using System.Collections;

public class AttackCommand : Command
{
    public Entity entity;

    public AttackCommand(Entity entity)
    {
        this.entity = entity;
    }

    public override IEnumerator Execute(Action callback)
    {
        yield return entity.StartCoroutine(entity.Attack());
        callback?.Invoke();
        Delete();
        yield break;
    }

}
