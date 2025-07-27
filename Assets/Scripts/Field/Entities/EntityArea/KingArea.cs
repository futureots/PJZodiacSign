using System.Collections.Generic;
using UnityEngine;

public class KingArea : MonoBehaviour,IMoveArea, IAttackArea
{
    List<intVector2> area;

    private void Start()
    {
        area = new List<intVector2>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i == j && j == 0) continue;
                area.Add(new intVector2(i, j));
            }
        }
    }

    public List<Tile> GetAttackArea(Tile tile, bool isReflect)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var item in area)
        {
            var pos = isReflect ? tile.fieldPos + item : tile.fieldPos - item;
            var nextTile = tile.field.GetTile(pos);
            if (nextTile != null)
            {
                tiles.Add(nextTile);
            }
        }
        return tiles;
    }

    public List<Tile> GetMoveArea(Tile tile, bool isReflect)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var item in area)
        {
            var pos = isReflect ? tile.fieldPos + item : tile.fieldPos - item;
            var nextTile = tile.field.GetTile(pos);
            if (nextTile != null && nextTile.isEmpty)
            {
                tiles.Add(nextTile);
            }

        }
        return tiles;
    }
}
