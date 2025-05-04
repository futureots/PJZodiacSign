using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public BuffData buffData;
    public int turnCount;
    public BuffInstance(int count,BuffData data)
    {
        turnCount = count;
        buffData = data;
    }
    /// <summary>
    /// 버프 적용
    /// </summary>
    /// <param name="entity"></param>
    public void ApplyBuff(Entity entity)
    {
        buffData.ApplyBuff(entity,turnCount);
    }
    /// <summary>
    /// 턴 감소 및 버프 효과 업데이트
    /// </summary>
    public void UpdateBuff(Entity entity)
    {
        buffData.UpdateBuff(entity,turnCount);
    }
    /// <summary>
    /// 버프 제거
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveBuff(Entity entity)
    {
        buffData.RemoveBuff(entity,turnCount);
    }

}
