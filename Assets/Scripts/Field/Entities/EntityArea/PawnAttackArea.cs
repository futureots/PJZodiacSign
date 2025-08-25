using System.Collections.Generic;
using UnityEngine;

public class PawnAttackArea : MonoBehaviour, IAttackArea
{
    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect)
    {
        List<intVector2> list = new();
        for(int i= -1;i < 2; i += 2)
        {
            var pos = isReflect ? curPos - new intVector2(i, 1) : curPos +  new intVector2(i, 1);
            if (!Field.IsPositionValid(tiles, pos)) continue;
            list.Add(pos);
        }
        return list;
    }
}
