using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/Buff/BuffData")]
public abstract class BuffData : ScriptableObject
{
    /// <summary>버프 아이콘</summary>
    public Sprite buffIcon;
    /// <summary>
    /// 상태이상 적용
    /// </summary>
    public abstract void ApplyBuff(Entity entity, int count);
    /// <summary>
    /// 턴 종료시 상태효과 변화 및 업데이트
    /// </summary>
    public abstract void UpdateBuff(Entity entity,ref int count);
    /// <summary>
    /// 상태효과 제거
    /// </summary>
    /// <param name="entity"></param>
    public abstract void RemoveBuff(Entity entity, int count);

    /// <summary>
    /// 버프 효과가 존재할 경우 연장
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="currentCount"></param>
    /// <param name="count"></param>
    public abstract void ExtendBuff(Entity entity, ref int currentCount, int count);
}
