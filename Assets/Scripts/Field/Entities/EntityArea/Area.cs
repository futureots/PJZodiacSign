using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Area : ScriptableObject
{
    protected abstract List<intVector2> GetVector(int[,] tiles, intVector2 pos, intVector2 direction);

    public List<intVector2> GetVectors(int[,] tiles, intVector2 pos, intVector2 direction)
    {
        var list = GetVector(tiles, pos, direction).Where(tiles.IsValidPos).ToList();
        return list;
    }
}

