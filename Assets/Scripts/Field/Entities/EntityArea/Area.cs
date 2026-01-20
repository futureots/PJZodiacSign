using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Area : ScriptableObject
{
    protected abstract List<intVector2> GetVector(int[,] tiles, intVector2 pos, intVector2 direction);

    public List<intVector2> GetVectors(int[,] tiles, intVector2 pos, intVector2 direction)
    {
        var list = GetVector(tiles, pos, direction).Where(value => IsValidPos(tiles, value)).ToList();
        return list;

    }
    protected bool IsValidPos(int[,] tiles,intVector2 pos)
    {
        var y = tiles.GetLength(0);
        var x = tiles.GetLength(1);
        if (pos.x < 0 || pos.y < 0 || pos.x >= x || pos.y >= y) return false;
        return true;
    }
}

