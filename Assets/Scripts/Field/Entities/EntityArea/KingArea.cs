using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KingArea", menuName = "Scriptable Objects/Area/KingArea")]
public class KingArea : Area
{
    public int size;
    protected override List<intVector2> GetVector(int[,] tiles, intVector2 curPos, intVector2 direction)
    {
        List<intVector2> list = new List<intVector2>();
        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                if (i == 0 && j == 0) continue;
                var item = new intVector2(i, j);

                var pos = curPos + item * direction;
                list.Add(pos);
            }
        }
        return list;
    }
}
