using System.Collections.Generic;
using UnityEngine;

public class RookArea : MonoBehaviour, IMoveArea, IAttackArea
{

    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            while (true)
            {
                vector += direction[i];
                var pos = isReflect ? curPos + vector : curPos - vector;
                if (!Field.IsPositionValid(tiles, pos)) break;
                list.Add(pos);
                if (tiles[pos.y, pos.x] != 0) {
                    break;
                }
            }
        }

        return list;

    }


    public List<intVector2> GetMoveVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            while (true)
            {
                vector += direction[i];
                var pos = isReflect ? curPos + vector : curPos - vector;
                if (!Field.IsPositionValid(tiles, pos)) break;
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
