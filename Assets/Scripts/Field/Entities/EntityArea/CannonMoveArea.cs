using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CannonMoveArea", menuName = "Scriptable Objects/Area/CannonMoveArea")]
public class CannonMoveArea : Area
{

    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, intVector2 direction)
    {
        List<intVector2> list = new();
        intVector2[] directionList = { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new(0, 0);
            bool isOverEntity = false;
            for (int j = 0; j < 8; j++)
            {
                vector += directionList[i];
                var pos = curPos + vector * direction;
                if (!tiles.IsValidPos(pos)) break;
                if (tiles[pos.y, pos.x] != Field.EmptyTileIndex)
                {
                    if (isOverEntity) break;
                    isOverEntity = true;
                    continue;
                }
                if(isOverEntity)
                {
                    list.Add(pos);
                }

            }
        }

        return list;

    }


    
}
