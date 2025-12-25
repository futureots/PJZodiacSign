using UnityEngine;

public class AttackCommand : Command
{
    public Entity entity;

    public AttackCommand(Entity entity)
    {
        this.entity = entity;
    }

    public override void Execute()
    {
        entity.Attack();
    }
}
