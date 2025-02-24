using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Pig : Entity
{
    public override List<intVector2> GetAttackArea(intVector2 entityPos)
    {
        if (field == null) return new List<intVector2>();
        List<intVector2> area = new List<intVector2>();
        for (int i = 0; i < 2; i++)
        {
            var temp = new intVector2(1, i) + entityPos;
            area.Add(temp);
            var temp2 = new intVector2(-1, i) + entityPos;
            area.Add(temp2);
        }
        for (int i = 1; i < 4; i++)
        {
            var temp = new intVector2(0, i) + entityPos;
            area.Add(temp);
            var tile = field.GetTile(temp);
            if (tile == null) continue;
            if (tile.isOccupied) break;
        }
        return area;
    }
}
