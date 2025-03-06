using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Tiger : Entity
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
                    int k = 2;
                    while (true)
                    {
                        var entityTile = field.GetTile(vec * k + entityPos);
                        k++;
                        if (entityTile == null) break;
                        if (!entityTile.isOccupied) continue;
                        var backTile = field.GetTile(vec * k + entityPos);
                        if(backTile != null)
                        {
                            if(!backTile.isOccupied) list.Add(backTile);
                        }
                    }
                }
            }
        }


        return list;
    }
}
