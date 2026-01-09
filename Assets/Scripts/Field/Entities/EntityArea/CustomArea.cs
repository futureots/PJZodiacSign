using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CustomArea", menuName = "Scriptable Objects/Area/CustomArea")]
public class CustomArea : Area
{

    public List<intVector2> positions;
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 pos, bool isReflect)
    {
        var area = new List<intVector2>();
        
        foreach (var item in positions)
        {
            var position = isReflect ? pos - item : pos + item;
            area.Add(position);
        }
        return area;
    }

    
}
