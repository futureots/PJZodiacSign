using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Cow : Entity_Sheep
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        for(int i = -2; i <= 2; i++)
        {
            for(int j = -2; j <= 2; j++)
            {
                if (j == 0 && i == 0) continue;
                if(i*j < 2 && i*j > -2)
                {
                    var tile = field.GetTile(new intVector2(i, j)+entityPos);
                    if(tile != null)
                    {
                        list.Add(tile);
                    }
                }
            }
        }
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var vec = new intVector2(i, j);
                var tile = field.GetTile(vec + entityPos);
                if(tile == null) continue;
                if (tile.isEmpty)
                {
                    foreach(var item in GetBlockedArea(vec))
                    {
                        var blockedTile = field.GetTile(item + entityPos);
                        if(blockedTile != null)
                        {
                            list.Remove(blockedTile);
                        }
                    }
                }
            }
        }

        return list;
    }
    
}
