using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IActive
{
    /// <summary>
    /// 스킬 자동입력이 가능한지 확인하는 함수
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool CanSkillInput(Field field);
    /// <summary>
    /// 스킬 입력값 자동 할당 기능(값 선정은 랜덤)
    /// </summary>
    /// <param name="field">스킬을 사용할 필드</param>
    /// <returns></returns>
    public bool SetSkillInput(Field field);
    /// <summary>
    /// 콜백 함수 추가
    /// </summary>
    /// <param name="func"></param>
    public void AddCallback(Action<bool> func);
    /// <summary>
    /// 콜백 함수 초기화
    /// </summary>
    public void ClearCallback();
    /// <summary>
    /// 스킬 사용 래핑 함수
    /// </summary>
    /// <returns></returns>
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

}
