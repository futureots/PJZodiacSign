using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffManager : MonoBehaviour
{
    List<BuffInstance> _buffList;
    public List<BuffInstance> buffList
    {
        get
        {
            if (_buffList == null) _buffList = new List<BuffInstance>();
            return _buffList;
        }
    }
    /// <summary>
    /// 버프 추가
    /// </summary>
    /// <param name="buff">버프 데이터</param>
    /// <param name="count">버프 카운트</param>
    public void AddBuff(BuffData buff, int count)
    {
        var existBuff = _buffList.Find((x) => x.buffData.GetType() == buff.GetType());
        if (existBuff != null)
        {
            existBuff.ExtendBuff(count);
        }
        else
        {
            var instance = new BuffInstance(count, buff);
            _buffList.Add(instance);
            instance.ApplyBuff(gameObject);
        }
        Debug.Log(buffList.Count);
    }
    /// <summary>
    /// 버프 업데이트
    /// </summary>
    public void UpdateBuff()
    {
        foreach (var buff in _buffList)
        {
            buff.UpdateBuff();
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
            buff.RemoveBuff();
            _buffList.Remove(buff);
        }
    }
}
