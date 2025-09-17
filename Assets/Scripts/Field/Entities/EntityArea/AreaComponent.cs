using System.Collections.Generic;
using UnityEngine;

public class AreaComponent : MonoBehaviour
{
    #region Attack
    bool isSealed
    {
        get
        {
            return false;
        }
    }

    public List<intVector2> GetAttackVector(int[,] field, intVector2 pos, bool isReflect)
    {
        var attackArea = new List<intVector2>();
        if (isSealed)
        {
            return attackArea;
        }
        var list = GetComponents<IAttackArea>();


        foreach (var area in list)
        {
            var vectors = area.GetAttackVector(field, pos, isReflect);
            attackArea.AddRange(vectors);
        }
        return attackArea;
    }

    #endregion

    #region Move
    bool isRooted
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 기물의 이동 가능 좌표를 반환
    /// </summary>
    /// <returns>이동 가능 좌표의 배열</returns>
    public List<intVector2> GetMoveVector(int[,] field, intVector2 pos, bool isReflect)
    {
        var moveArea = new List<intVector2>();
        if (isRooted)
        {
            return moveArea;
        }
        
        if(TryGetComponent<IOccupant>(out var occupant))
        {
            var list = GetComponents<IMoveArea>();

            foreach (var area in list)
            {
                var vectors = area.GetMoveVector(field, pos, occupant.IsReflect);
                moveArea.AddRange(vectors);
            }

            moveArea.Add(pos);
        }

        
        return moveArea;
    }

    #endregion
}
