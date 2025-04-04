using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{
    Entity entity;
    Tile tile;
    public MoveCommand(Entity entity, Tile tile)
    {
        this.entity = entity;
        this.tile = tile;
    }

    public override void Execute()
    {
        entity.MoveToTile(tile);
    }
}
