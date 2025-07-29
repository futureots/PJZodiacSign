using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackArea
{
    /// <summary>
    /// 시작 위치를 중심으로 공격 범위를 반환한다.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="curPos"></param>
    /// <param name="isReflect"></param>
    /// <returns></returns>
    public List<intVector2> GetAttackVector(int[,] tiles, intVector2 curPos, bool isReflect);
}
