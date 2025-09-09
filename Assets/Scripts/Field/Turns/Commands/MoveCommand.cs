using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveCommand : Command
{
    Entity entity;
    Tile tile;
    public MoveCommand(Entity entity, Tile tile)
    {
        selecterObjects = new();
        this.entity = entity;
        this.tile = tile;
    }

    public override void Execute()
    {
        var result = entity.MoveSequence(tile, true);
        base.Execute();
    }

    public override string ToString()
    {
        return $"{entity.data.productName}({entity.curTile.fieldPos})이 {tile.fieldPos}으로 이동";
    }
}
