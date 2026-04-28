using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositeArea", menuName = "Scriptable Objects/Area/CompositeArea")]
public class CompositeArea : Area
{
    public List<Area> areas;
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 pos, intVector2 direction)
    {
        List<intVector2> list = new();
        foreach (var area in areas)
        {
            list.AddRange(area.GetVectors(tiles,pos,direction));
        }
        list = list.Distinct().ToList();
        return list;
    }
}
