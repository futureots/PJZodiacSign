using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다른 기물 위치에 관계없이 원하는 위치에 이동 가능한 엔티티
/// </summary>
public class Entity_Jumper : Oldity
{
    public List<intVector2> area;
    public override List<Tile> GetAttackArea(intVector2 entityPos)
    {
        var absArea = new List<Tile>();
        if (field == null) return absArea;
        foreach (var pos in area)
        {
            var tile = field.GetTile(pos * negative + entityPos);
            if (tile != null)
            {
                absArea.Add(tile);
            }
        }
        return absArea;
    }
}
