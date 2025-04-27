using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect
{
    /// <summary>
    /// 상태이상 적용
    /// </summary>
    public void ApplyEffect(Entity entity);
    /// <summary>
    /// 턴 종료시 상태이상 적용 및 업데이트
    /// </summary>
    public void OnTurnEnd(Entity entity);
    /// <summary>
    /// 상태이상 지속시간 만료 
    /// </summary>
    public bool IsExpired();
    /// <summary>
    /// 상태이상 턴 중첩
    /// </summary>
    /// <param name="count">턴 증가량</param>
    public void Refresh(int count);
}
