using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomArea", menuName = "Scriptable Objects/Area/CustomArea")]
public class CustomArea : Area
{

    public List<intVector2> positions;
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 pos, bool isReflect)
    {
        var area = new List<intVector2>();
        
        foreach (var item in positions)
        {
            area.Add(isReflect ? item + pos : item - pos);
        }
        return area;
    }

    
}
