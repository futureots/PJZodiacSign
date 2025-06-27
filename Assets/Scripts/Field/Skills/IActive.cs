using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IActive
{
    
    public void AddCallback(Action<bool> func);
    public void ClearCallback();
    public bool ExecuteSequence();
    /// <summary>
    /// 스킬을 사용하는 함수
    /// </summary>
    public void Activate();
    /// <summary>
    /// 스킬이 사용가능한지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    
    public bool IsActable()
    {
        return true;
    }
    /// <summary>
    /// field의 변수가 필요한 값을 가지고 있는지 확인하는 함수 내부적으로 각 변수에 대한 제한 추가
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool IsValidInput(FieldInfo field);

    /// <summary>
    /// 스킬 대상들을 null로 바꾸는 함수(유지할 경우 넣을 필요 없음)
    /// </summary>
    public void Reinitialize();
}
