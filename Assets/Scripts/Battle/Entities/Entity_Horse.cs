using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Horse : Oldity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        intVector2[] temp = { new intVector2(0, 1), new intVector2(1, 0), new intVector2(0, -1), new intVector2(-1, 0) };
        foreach (var item in temp)
        {
            for (int i = 1; i <= 4; i++)
            {
                var tile = field.GetTile(item * i+entityPos);
                if (tile == null) break;
                list.Add(tile);
                if (tile.isEmpty) break;
            }
        }
        return list;
    }

}
