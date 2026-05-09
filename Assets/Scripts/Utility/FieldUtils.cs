using System.Collections.Generic;
using UnityEngine;

public static class FieldUtils
{
    /// <summary>
    /// 타일 리스트의 점거되지 않은 빈 타일 리스트 반환
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Tile> GetEmptyTiles(this List<Tile> tiles)
    {
        var emptyTiles = new List<Tile>();
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty)
            {
                emptyTiles.Add(tile);
            }
        }
        return emptyTiles;
    }
    
    /// <summary>
    /// 해당 위치가 타일 리스트에 포함되는지 확인
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static bool IsValidPos(this int[,] tiles,intVector2 pos)
    {
        var y = tiles.GetLength(0);
        var x = tiles.GetLength(1);
        if (pos.x < 0 || pos.y < 0 || pos.x >= x || pos.y >= y) return false;
        return true;
    }
    
}
