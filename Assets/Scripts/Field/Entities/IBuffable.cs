using System.Collections.Generic;
using UnityEngine;

public interface IBuffable
{
    /// <summary>
    /// 버프 적용
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="count"></param>
    public void AddBuff(BuffData buff, int count);

    /// <summary>
    /// 버프 업데이트
    /// </summary>
    public void UpdateBuff();

    /// <summary>
    /// 버프 제거
    /// </summary>
    public void RemoveBuff();
}
