using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Tiger : Entity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        for(int i = 0;i< 4; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0) continue;
                var tile = field.GetTile(new intVector2(j, i) * negative + entityPos);
                if(tile != null)
                {
                    list.Add(tile);
                }
            }
        }
        return list;
    }
}
