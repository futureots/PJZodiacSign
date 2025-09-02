using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveArea
{
    /// <summary>
    /// 시작 위치를 중심으로 이동 범위를 반환한다.
    /// </summary>
    public List<intVector2> GetMoveVector(int[,] tiles, intVector2 curPos, bool isReflect);

}
