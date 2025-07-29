using System.Collections.Generic;
using UnityEngine;

public class KingArea : MonoBehaviour,IMoveArea, IAttackArea
{
    List<intVector2> area;

    private void Start()
    {
        area = new List<intVector2>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i == j && j == 0) continue;
                area.Add(new intVector2(i, j));
            }
        }
    }

    public List<intVector2> GetMoveVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> vectors = new List<intVector2>();
        foreach (var item in area)
        {
            var pos = isReflect ? curPos + item : curPos - item;
            if (!Field.isValidPos(tiles, pos)) continue;
            if (tiles[pos.y, pos.x] != 0) continue;
            vectors.Add(pos);
        }
        return vectors;
    }



    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> vectors = new List<intVector2>();
        foreach (var item in area)
        {
            var pos = isReflect ? curPos + item : curPos - item;
            if (!Field.isValidPos(tiles, pos)) continue;
            vectors.Add(pos);
        }
        return vectors;
    }
}
