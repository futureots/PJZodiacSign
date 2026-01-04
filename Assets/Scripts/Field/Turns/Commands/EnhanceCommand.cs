using System;
using System.Collections;

public class EnhanceCommand : Command
{

    Entity baseEntity;
    Entity subEntity;
    public EnhanceCommand(Entity baseEntity, Entity subEntity)
    {
        this.baseEntity = baseEntity;
        this.subEntity = subEntity;
    }

    public override IEnumerator Execute(Action callback = null)
    {

        baseEntity.Level += 1;
        subEntity.CurTile.ClearOccupant();

        callback?.Invoke();
        yield break;
    }
}
