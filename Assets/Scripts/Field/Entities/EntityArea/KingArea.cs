using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KingArea", menuName = "Scriptable Objects/Area/KingArea")]
public class KingArea : Area
{

    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new List<intVector2>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;
                var item = new intVector2(i, j);

                var pos = isReflect ? curPos + item : curPos - item;
                list.Add(pos);
            }
        }
        return list;
    }
}
