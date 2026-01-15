using System;
using System.Collections;

public sealed record MoveCommand(Entity Entity, Tile To, Tile From) : Command
{
    public MoveCommand(Entity entity, Tile To) : this(entity, To, entity.CurTile)
    {
        
    }
    
    public override IEnumerator Execute(Action callback)
    {
        Entity.Move(To);
        callback?.Invoke();
        Delete();
        yield break;
    }

    public override string ToString()
    {
        var prevPos = From.fieldPos;
        var prevText = $"( {(char)((prevPos.x) + 'A')}, {prevPos.y + 1} )";

        var pos = To.fieldPos;
        var posText = $"( {(char)((pos.x) + 'A')}, {pos.y + 1} )";

        return $"{Entity.baseData.productName} {prevText} 이 {posText}으로 이동";
    }
}
