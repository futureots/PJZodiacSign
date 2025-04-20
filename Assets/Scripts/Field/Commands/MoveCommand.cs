using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
public class MoveCommand : Command
{
    Battle.Entity entity;
    Tile tile;
    public MoveCommand(Battle.Entity entity, Tile tile)
    {
        selecterObjects = new();
        this.entity = entity;
        this.tile = tile;
    }

    public override void Execute()
    {
        entity.MoveTo(tile);
    }


}
