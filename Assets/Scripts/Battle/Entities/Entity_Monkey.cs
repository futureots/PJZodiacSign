using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity_Monkey : Oldity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (i == 0 && j == 0) continue;
                var tile = field.GetTile(new intVector2(i, j)+entityPos);
                if (tile == null) continue;
                if (tile.isOccupied)
                {
                    list.AddRange(GetSurroundTiles(tile));
                }
            }
        }
        return list;
    }
    List<Tile> GetSurroundTiles(Tile tile)
    {
        var list = new List<Tile>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                var surroundTile = field.GetTile(new intVector2(i, j) + tile.fieldPos);
                if (surroundTile == null) continue;
                list.Add(surroundTile);
            }
        }
        return list;
    }
}
