using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Pig : Entity
{
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        if (field == null) return new List<Tile>();
        List<Tile> area = new List<Tile>();
        for (int i = 0; i < 2; i++)
        {
            var temp = field.GetTile(new intVector2(1, i) * negative + entityPos);
            if (temp !=null)
            {
                area.Add(temp);
            }
            var temp2 = field.GetTile(new intVector2(-1, i) * negative + entityPos);
            if (temp2 != null)
            {
                area.Add(temp2);
            }
            
        }
        for (int i = 1; i < 4; i++)
        {
            var temp = field.GetTile(new intVector2(0, i) * negative + entityPos);
            if (temp == null) break;
            area.Add(temp);
            if (temp.isOccupied) break;
        }
        return area;
    }
}
