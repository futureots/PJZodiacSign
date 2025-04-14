using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public bool ExecuteSkillSequence()
    {
        var isActable = IsActable();
        if (isActable)
        {
            Activate();
            Reinitialize();
        }
        return isActable;
    }
    /// <summary>
    /// 스킬을 사용하는 함수
    /// </summary>
    public void Activate();
    /// <summary>
    /// 스킬이 사용가능한지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsActable();
    
    /// <summary>
    /// 스킬 대상들을 null로 바꾸는 함수(유지할 경우 넣을 필요 없음)
    /// </summary>
    public void Reinitialize();
}
