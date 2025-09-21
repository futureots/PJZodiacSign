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
        prevTile = entity.CurTile;
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
        var prevText = $"( {(char)((prevPos.x - 1) + 'A')}, {prevPos.y} )";

        var pos = tile.fieldPos;
        var posText = $"( {(char)((pos.x - 1) + 'A')}, {pos.y} )";

        return $"{entity.baseData.productName} {prevText} 이 {posText}으로 이동";
    }
}
