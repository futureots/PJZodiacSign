using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 버프를 받는 객체의 버프를 관리하는 클래스
/// </summary>
public class BuffManager : MonoBehaviour
{
    List<BuffInstance> _buffList;
    public List<BuffInstance> BuffList
    {
        get
        {
            if (_buffList == null) _buffList = new List<BuffInstance>();
            return _buffList;
        }
    }
    /// <summary>
    /// 버프 추가(리스트에 버프가 이미 존재할 경우 해당 카운트만큼 연장 또는 덮어쓰기)
    /// </summary>
    /// <param name="buff">버프 데이터</param>
    /// <param name="count">버프 카운트</param>
    public void AddBuff(BuffData buff, int count)
    {
        var existBuff = _buffList.Find((x) => x.buffData.id == buff.id);
        if (existBuff != null)
        {
            existBuff.ExtendBuff(gameObject, count);
        }
        else
        {
            var instance = new BuffInstance(count, buff);
            if (instance.TryApplyBuff(gameObject))
            {
                _buffList.Add(instance);
            }
        }
        EditorLogger.Print(BuffList.Count);
    }
    /// <summary>
    /// 버프 업데이트
    /// </summary>
    public void UpdateBuff()
    {
        foreach (var buff in BuffList)
        {
            buff.UpdateBuff(gameObject);
        }
    }

    /// <summary>
    /// 버프 제거
    /// </summary>
    public void RemoveBuff()
    {
        var list = _buffList.Where((buff) => buff.IsExpired()).ToList();
        foreach (var buff in list)
        {
            buff.RemoveBuff(gameObject);
            _buffList.Remove(buff);
        }
    }
}
