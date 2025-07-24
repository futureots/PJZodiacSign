using System.Collections.Generic;
using UnityEngine;

public class RookArea : MonoBehaviour, IMoveArea, IAttackArea
{
    public List<Tile> GetAttackArea(Tile tile)
    {
        List<Tile> list = new List<Tile>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };
        
        for(int i = 0; i < 4; i++)
        {
            intVector2 curPos = new intVector2(0, 0);
            while (true)
            {
                curPos += direction[i];
                var nextTile = tile.field.GetTile(tile.fieldPos + curPos);
                if (nextTile != null)
                {
                    if (nextTile.isEmpty || nextTile.occupiedObject == gameObject)
                    {
                        list.Add(nextTile);
                    }
                    else
                    {
                        var other = nextTile.occupiedObject.GetComponent<Team>();
                        if (other == null || !GetComponent<Team>().isAlly(other))
                        {
                            list.Add(nextTile);
                        }
                        break;
                    }
                }
                else break;
            }
        }
        

        return list;
    }

    public List<Tile> GetMoveArea(Tile tile)
    {
        List<Tile> list = new List<Tile>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 curPos = new intVector2(0, 0);
            while (true)
            {
                curPos += direction[i];
                var nextTile = tile.field.GetTile(tile.fieldPos + curPos);
                if (nextTile != null)
                {
                    if (nextTile.isEmpty || nextTile.occupiedObject == gameObject)
                    {
                        list.Add(nextTile);
                    }
                    else
                    {
                        break;
                    }
                }
                else break;
            }
        }
        list.Add(tile);
        return list;
    }
}
