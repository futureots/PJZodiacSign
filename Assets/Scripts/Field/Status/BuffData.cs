using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "BuffData",menuName = "DefaultBuffData")]
public abstract class BuffData : ScriptableObject
{
    /// <summary>
    /// 상태이상 적용
    /// </summary>
    public abstract void ApplyBuff(Entity entity, int count);
    /// <summary>
    /// 턴 종료시 상태효과 변화 및 업데이트
    /// </summary>
    public abstract void UpdateBuff(Entity entity, int count);
    /// <summary>
    /// 상태효과 제거
    /// </summary>
    /// <param name="entity"></param>
    public abstract void RemoveBuff(Entity entity, int count);
}
