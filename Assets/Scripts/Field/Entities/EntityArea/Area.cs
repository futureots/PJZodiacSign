using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Area : ScriptableObject
{
    /// <summary>
    /// 점령중인 타일을 포함할건지 확인하는 변수
    /// </summary>
    public bool isContainOccupiedPos;
    protected abstract List<intVector2> GetVector(int[,] tiles, intVector2 pos, bool isReflect);

    public List<intVector2> GetVectors(int[,] tiles, intVector2 pos, bool isReflect)
    {
        var list = GetVector(tiles, pos, isReflect).Where(value => IsValidPos(tiles, value)).ToList();
        if (isContainOccupiedPos)
        {
            return list;
        }
        else
        {
            return list.Where(x => tiles[x.y, x.x] != 0).ToList();
        }
        
    }
    protected bool IsValidPos(int[,] tiles,intVector2 pos)
    {
        var y = tiles.GetLength(0);
        var x = tiles.GetLength(1);
        if (pos.x < 0 || pos.y < 0 || pos.x >= x || pos.y >= y) return false;
        return true;
    }
}
