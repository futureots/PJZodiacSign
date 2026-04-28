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
    public bool TryApplyBuff(GameObject target)
    {
        return buffData.ApplyBuff(target,count);
    }
    /// <summary>
    /// 턴 감소 및 버프 효과 업데이트
    /// </summary>
    public void UpdateBuff(GameObject target)
    {
        buffData.UpdateBuff(target,ref count);
    }
    /// <summary>
    /// 버프 제거
    /// </summary>
    /// <param name="target"></param>
    public void RemoveBuff(GameObject target)
    {
        buffData.RemoveBuff(target,count);
    }
    /// <summary>
    /// 버프 지속 효과가 끝났는지 확인
    /// </summary>
    /// <returns></returns>
    public bool IsExpired() => count == 0;

    /// <summary>
    /// 버프 연장
    /// </summary>
    /// <param name="target"></param>
    /// <param name="count"></param>
    public void ExtendBuff(GameObject target, int count)
    {
        buffData.ExtendBuff(target, ref this.count, count);
    }
}
