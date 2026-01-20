using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CustomArea", menuName = "Scriptable Objects/Area/CustomArea")]
public class CustomArea : Area
{

    public List<intVector2> positions;
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, intVector2 direction)
    {
        var area = new List<intVector2>();
        
        foreach (var item in positions)
        {
            var pos = curPos + item * direction;
            area.Add(pos);
        }
        return area;
    }

    
}
