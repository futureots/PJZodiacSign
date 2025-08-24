using System.Collections.Generic;
using UnityEngine;

public class KnightArea : MonoBehaviour, IAttackArea, IMoveArea
{
    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        var area = new List<intVector2>();
        var list = GetVector(curPos);
        foreach (var tile in list)
        {
            if (!Field.IsPositionValid(tiles, tile)) continue;
            area.Add(tile);
        }
        return area;
    }

    public List<intVector2> GetMoveVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        var area = new List<intVector2>();
        var list = GetVector(curPos);
        foreach (var tile in list)
        {
            if (!Field.IsPositionValid(tiles, tile)) continue;
            area.Add(tile);
        }
        return area;
    }
    
    List<intVector2> GetVector(intVector2 curPos)
    {
        List<intVector2> vec = new List<intVector2>() { new intVector2(1, 2), new intVector2(2, 1), new intVector2(-1, 2), new intVector2(-2, 1), new intVector2(-1, -2), new intVector2(-2, -1), new intVector2(1, -2), new intVector2(2, -1) };
        List<intVector2> result = new List<intVector2>();
        foreach (var item in vec) 
        {
            var pos = item + curPos;
            result.Add(pos);
        }
        return result;
    }
    
}
