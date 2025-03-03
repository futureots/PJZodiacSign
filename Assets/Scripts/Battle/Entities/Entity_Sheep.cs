using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Sheep : Entity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        if (field == null) return list;
        for (int i = -1; i <= 2; i++)
        {
            var length = i == 0 || i == 1 ? 2 : 1;
            for (int j = -length; j <= length; j++)
            {
                if (i == 0 && j == 0) continue;
                var tile = field.GetTile(new intVector2(j, i) * negative + entityPos);
                if(tile != null) list.Add(tile);
            }
        }
        for (int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i== 0 && j == 0) continue;
                var t = new intVector2(i, j);
                var tile = field.GetTile(entityPos + t);
                if (tile == null) continue;
                if (tile.isOccupied)
                {
                    foreach (var item in GetBlockedArea(t))
                    {
                        var blockedTile = field.GetTile(item+ entityPos);
                        if (blockedTile != null)
                        {
                            list.Remove(blockedTile);
                        }
                    }
                }
            }
        }
        


        return list;
    }
    List<intVector2> GetBlockedArea(intVector2 blockPos)
    {
        int mul = blockPos.x * blockPos.y;
        var pos = new intVector2(blockPos.x, blockPos.y) * 2;
        List<intVector2> list = new List<intVector2>();
        switch (mul)
        {
            case 0:
                list = new List<intVector2>() {pos ,pos + new intVector2(blockPos.y , blockPos.x), pos - new intVector2(blockPos.y, blockPos.x) };
                break;
            case 1:
            case -1:
                list =  new List<intVector2>() { pos, pos - new intVector2(0, blockPos.y), pos - new intVector2(blockPos.x, 0) };
                break;
        }
        return list;
    }
}
