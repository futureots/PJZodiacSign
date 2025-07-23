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
