using System.Collections.Generic;
using UnityEngine;

public class ObjectArea : MonoBehaviour,IMoveArea, IAttackArea
{
    public List<intVector2> area;

    public List<Tile> GetAttackArea(Tile curTile)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var item in area)
        {
            Tile tile = curTile.field.GetTile(item + curTile.fieldPos);
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }
        return tiles;
    }

    public List<Tile> GetMoveArea(Tile curTile)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var item in area)
        {
            Debug.Log(curTile);
            Debug.Log(curTile.field);
            Tile tile = curTile.field.GetTile(item + curTile.fieldPos);
            if (tile != null && tile.isEmpty)
            {
                tiles.Add(tile);
            }
            
        }
        return tiles;
    }
}
