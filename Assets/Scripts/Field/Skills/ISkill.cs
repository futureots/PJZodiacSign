using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    /// <summary>
    /// 스킬을 사용하는 함수
    /// </summary>
    public void Activate();
    /// <summary>
    /// 스킬이 사용가능한지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsActable();
}
