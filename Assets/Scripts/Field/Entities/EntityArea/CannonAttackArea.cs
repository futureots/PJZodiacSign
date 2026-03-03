using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CannonAttackArea", menuName = "Scriptable Objects/Area/CannonAttackArea")]
public class CannonAttackArea : Area
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
                if (!tiles.IsValidPos(pos)) break;
                if (tiles[pos.y, pos.x] != Field.EmptyTileIndex)
                {
                    list.Add(pos);
                    break;
                }
            }
        }

        return list;

    }


    
}
