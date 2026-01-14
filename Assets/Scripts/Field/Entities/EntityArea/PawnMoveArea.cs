using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PawnMoveArea", menuName = "Scriptable Objects/Area/PawnMoveArea")]
public class PawnMoveArea : Area
{
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {

        List<intVector2> area = new();
        intVector2 vector = new intVector2(0, 0);
        for (int i = 0; i < 2; i++)
        {
            vector += new intVector2(0, 1);
            var pos = isReflect ? curPos - vector : curPos + vector;
            if (!Field.IsPositionValid(tiles, pos)) break;
            if (tiles[pos.y, pos.x] != 0)
            {
                break;
            }
            area.Add(pos);
        }

        return area;
    }
}
