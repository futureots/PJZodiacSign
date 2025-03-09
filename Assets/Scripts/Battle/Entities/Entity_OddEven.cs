using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_OddEven : Entity
{
    public bool isOddTile;

    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var list = new List<Tile>();
        for(int i = -2; i <= 2; i++)
        {
            for(int j = -2; j <= 2; j++)
            {
                if(i == 0 && j == 0) continue;
                if(((i +j) %2 == 0  && !isOddTile) || ((i+j)%2 != 0 && isOddTile))
                {
                    var tile = field.GetTile(new intVector2(i, j)+entityPos);
                    if(tile != null)
                    {
                        list.Add(tile);
                    }
                }
            }
        }
        return list;
    }
}
