using UnityEngine;

//[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/Buff/BuffData")]
public abstract class BuffData : ScriptableObject
{
    public string id;
    public string buffName;
    /// <summary>버프 아이콘</summary>
    public Sprite buffIcon;
    /// <summary>
    /// 상태이상 적용 성공 시 true 실패 시 false 반환
    /// </summary>
    public abstract bool ApplyBuff(GameObject target, int count);
    /// <summary>
    /// 턴 종료시 상태효과 변화 및 업데이트
    /// </summary>
    public abstract void UpdateBuff(GameObject target,ref int count);
    /// <summary>
    /// 상태효과 제거
    /// </summary>
    /// <param name="entity"></param>
    public abstract void RemoveBuff(GameObject target, int count);

    /// <summary>
    /// 버프 효과가 존재할 경우 연장
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="currentCount"></param>
    /// <param name="count"></param>
    public abstract void ExtendBuff(GameObject target, ref int currentCount, int count);
}
