using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveCommand : Command
{
    Tile prevTile;
    Entity entity;
    Tile tile;
    public MoveCommand(Entity entity, Tile tile)
    {
        selecterObjects = new();
        this.entity = entity;
        prevTile = entity.curTile;
        this.tile = tile;
    }

    public override void Execute()
    {
        var result = entity.Move(tile);
        base.Execute();
    }

    public override string ToString()
    {
        var prevPos = prevTile.fieldPos;
        var prevText = $"( {(char)((prevPos.x) + 'A')}, {prevPos.y + 1} )";

        var pos = tile.fieldPos;
        var posText = $"( {(char)((pos.x) + 'A')}, {pos.y + 1} )";

        return $"{entity.baseData.productName} {prevText} 이 {posText}으로 이동";
    }
}
