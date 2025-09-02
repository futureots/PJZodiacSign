using System;
using UnityEngine;

public class ItemData : AbstractData
{
    public string description;

    /// <summary>
    /// 아이템 데이터 기반 인스턴스 제작하기
    /// </summary>
    /// <returns></returns>
    public virtual ItemInstance CreateInstance()
    {
        return new ItemInstance(this);
    }

    /// <summary>
    /// 해당 아이템이 사용가능한 페이즈 타입
    /// </summary>
    public PhaseType useType;

}
