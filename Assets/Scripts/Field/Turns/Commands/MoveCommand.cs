using System;
using System.Collections;

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
        selecterObjects = new();
    }

    public override IEnumerator Execute(Action callback)
    {
        var result = entity.Move(tile);
        yield return null;
        callback?.Invoke();
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
