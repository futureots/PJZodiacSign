using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Snake : Entity
{
    public List<intVector2> area;
    public override List<intVector2> GetAttackArea(intVector2 entityPos)
    {
        var absArea = new List<intVector2>();
        foreach (var pos in area)
        {
            absArea.Add(pos + entityPos);
        }
        return absArea;
    }
}
