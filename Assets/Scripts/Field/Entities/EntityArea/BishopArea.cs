using System.Collections.Generic;
using UnityEngine;

public class BishopArea : MonoBehaviour, IAttackArea, IMoveArea
{
    public List<Tile> GetAttackArea(Tile tile, bool isReflect)
    {
        List<Tile> list = new List<Tile>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 1), new intVector2(-1, 1), new intVector2(-1, -1), new intVector2(1, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 curPos = new intVector2(0, 0);
            while (true)
            {
                curPos += direction[i];
                var pos = isReflect ? tile.fieldPos + curPos : tile.fieldPos - curPos;
                var nextTile = tile.field.GetTile(pos);
                if (nextTile != null)
                {
                    list.Add(nextTile);
                    if (!nextTile.isEmpty && nextTile.occupiedObject != gameObject) break;
                }
                else break;
            }
        }
        return list;
    }

    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 1), new intVector2(-1, 1), new intVector2(-1, -1), new intVector2(1, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            while (true)
            {
                vector += direction[i];
                var pos = isReflect ? curPos + vector : curPos - vector;
                if (!Field.isValidPos(tiles, pos)) break;
                list.Add(pos);
                if (tiles[pos.y, pos.x] != 0)
                {
                    break;
                }
            }
        }

        return list;
    }

    public List<Tile> GetMoveArea(Tile tile, bool isReflect)
    {
        List<Tile> list = new List<Tile>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 1), new intVector2(-1, 1), new intVector2(-1, -1), new intVector2(1, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 curPos = new intVector2(0, 0);
            while (true)
            {
                curPos += direction[i];
                var pos = isReflect ? tile.fieldPos + curPos : tile.fieldPos - curPos;
                var nextTile = tile.field.GetTile(pos);
                if (nextTile != null)
                {
                    if (!nextTile.isEmpty && nextTile.occupiedObject != gameObject) break;
                    list.Add(nextTile);
                }
                else break;
            }
        }
        list.Add(tile);
        return list;
    }

    public List<intVector2> GetMoveVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 1), new intVector2(-1, 1), new intVector2(-1, -1), new intVector2(1, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            while (true)
            {
                vector += direction[i];
                var pos = isReflect ? curPos + vector : curPos - vector;
                if (!Field.isValidPos(tiles, pos)) break;
                if (tiles[pos.y, pos.x] != 0)
                {
                    break;
                }
                list.Add(pos);
            }
        }

        return list;
    }
}
