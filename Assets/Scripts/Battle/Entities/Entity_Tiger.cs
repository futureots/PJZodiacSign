using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Tiger : Oldity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var vec = new intVector2(i, j);
                var tile = field.GetTile(vec + entityPos);
                if(tile != null)
                {
                    list.Add(tile);
                }
            }
        }
        return list;
    }

    public override List<Tile> GetMoveArea(intVector2 entityPos, bool hasOriginTile = true)
    {
        var list = GetAttackArea(entityPos);
        for(int i= -1;i <= 1; i++)
        {
            for (int j= -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var vec = new intVector2(i, j);
                int k = 1;
                while (true)
                {
                    var entityTile = field.GetTile(vec * k + entityPos);
                    if (entityTile == null) break;
                    k++;
                    if (entityTile.isEmpty)
                    {
                        list.Remove(entityTile);
                        var backTile = field.GetTile(vec * k + entityPos);
                        if (backTile == null) break;
                        if (backTile.isEmpty) break;
                        list.Add(backTile);
                        break;
                    }
                }

            }
        }
        if (hasOriginTile) list.Add(curTile);
        return list;
    }
}
