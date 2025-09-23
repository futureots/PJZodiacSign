using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RookArea", menuName = "Scriptable Objects/Area/RookArea")]
public class RookArea : Area
{

    protected override List<intVector2> GetVector(int[,] tiles, intVector2 pos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        intVector2[] direction = new intVector2[] { new intVector2(1, 0), new intVector2(0, 1), new intVector2(-1, 0), new intVector2(0, -1) };

        for (int i = 0; i < 4; i++)
        {
            intVector2 vector = new intVector2(0, 0);
            for (int j = 0; j < 8; j++)
            {
                vector += direction[i];
                var position = isReflect ? pos + vector : pos - vector;
                if (!IsValidPos(tiles, position)) break;
                list.Add(position);
                if (tiles[position.y, position.x] != 0)
                {
                    break;
                }

            }
        }

        return list;

    }


    
}
