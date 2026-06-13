using System;
using System.Collections;
using UnityEngine;

public sealed record MoveCommand(Entity Entity, Tile To, Tile From, bool IsWarp) : Command
{
    public MoveCommand(Entity entity, Tile To, bool IsWarp) : this(entity, To, entity.CurTile, IsWarp)
    {
        
    }
    
    public override IEnumerator Execute()
    {
        if (IsWarp)
        {
            Entity.Move(To);
        }
        else
        {
            var list = Entity.GetMoveArea();
            if (list.Contains(To))
            {
                Entity.Move(To);
            }
            
        }
        yield return new WaitForSeconds(0.5f);
        Delete();
        yield break;
    }

    public override string ToString()
    {
        var prevPos = From.fieldPos;
        var prevText = $"( {(char)((prevPos.x) + 'A')}, {prevPos.y + 1} )";

        var pos = To.fieldPos;
        var posText = $"( {(char)((pos.x) + 'A')}, {pos.y + 1} )";

        return $"{Entity.baseData.ProductName}{prevText} -> {posText} 이동";
    }
    public override bool IsOverlap(Command cmd)
    {
        if (cmd is MoveCommand mvCmd)
        {
            if (mvCmd.Entity == Entity)
            {
                return true;
            }
            else if (mvCmd.To == To)
            {
                return true;
            }
        }
        else if (cmd is SkillCommand skCmd)
        {
            if (Entity.skill == skCmd.Skill)
            {
                return true;
            }
        }
        return false;
    }
}
