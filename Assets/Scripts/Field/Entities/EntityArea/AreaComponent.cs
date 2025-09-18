using System.Collections.Generic;
using UnityEngine;

public class AreaComponent : MonoBehaviour
{

    public void SetArea(List<Area> moveArea, List<Area> attackArea)
    {
        this.moveArea = moveArea;
        this.attackArea = attackArea;
    }

    #region Attack
    public int sealCount;
    bool isSealed
    {
        get
        {
            return sealCount>0;
        }
    }

    public List<Area> attackArea;

    public List<intVector2> GetAttackVector(int[,] field, intVector2 pos, bool isReflect)
    {
        var list = new List<intVector2>();
        if (isSealed)
        {
            return list;
        }
        
        foreach (var area in attackArea)
        {
            var vectors = area.GetVectors(field, pos, isReflect);
            list.AddRange(vectors);
        }
        return list;
    }

    #endregion

    #region Move

    public int rootCount;
    bool isRooted
    {
        get
        {
            return rootCount>0;
        }
    }

    public List<Area> moveArea;
    /// <summary>
    /// 기물의 이동 가능 좌표를 반환
    /// </summary>
    /// <returns>이동 가능 좌표의 배열</returns>
    public List<intVector2> GetMoveVector(int[,] field, intVector2 pos, bool isReflect)
    {
        var list = new List<intVector2>();
        if (isRooted)
        {
            return list;
        }

        foreach (var area in moveArea)
        {
            var vectors = area.GetVectors(field, pos, isReflect);
            list.AddRange(vectors);
        }
        return list;
    }

    #endregion
}
