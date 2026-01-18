using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RookArea", menuName = "Scriptable Objects/Area/RookArea")]
public class RookArea : Area
{

    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, intVector2 direction)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] directionList = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            for (int j = 0; j < 8; j++)
            {
                vector += directionList[i];
                var pos = curPos + vector * direction;
                if (!IsValidPos(tiles, pos)) break;
                list.Add(pos);
                if (tiles[pos.y, pos.x] != Field.EmptyTileIndex)
                {
                    break;
                }

            }
        }

        return list;

    }


    
}
