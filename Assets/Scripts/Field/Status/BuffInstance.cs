using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public BuffData buffData;
    public int count;
    public BuffInstance(int count,BuffData data)
    {
        this.count = count;
        buffData = data;
    }
    /// <summary>
    /// 버프 적용
    /// </summary>
    /// <param name="entity"></param>
    public void ApplyBuff(Entity entity)
    {
        buffData.ApplyBuff(entity,count);
    }
    /// <summary>
    /// 턴 감소 및 버프 효과 업데이트
    /// </summary>
    public void UpdateBuff(Entity entity)
    {
        buffData.UpdateBuff(entity,ref count);
    }
    /// <summary>
    /// 버프 제거
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveBuff(Entity entity)
    {
        buffData.RemoveBuff(entity,count);
    }
    /// <summary>
    /// 버프 지속 효과가 끝났는지 확인
    /// </summary>
    /// <returns></returns>
    public bool IsExpired()
    {
        return count == 0;
    }

    /// <summary>
    /// 버프 연장
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="count"></param>
    public void ExtendBuff(Entity entity,int count)
    {
        buffData.ExtendBuff(entity, ref this.count, count);
    }
}
