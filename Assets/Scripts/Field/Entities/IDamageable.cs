using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// 이 객체를 damage만큼 피해를 입힌다.
    /// </summary>
    /// <param name="damage">피해량</param>
    public void Damaged(int damage);
    /// <summary>
    /// 이 객체를 amount만큼 회복한다.
    /// </summary>
    /// <param name="amount">회복량</param>
    public void Healed(int amount);
    /// <summary>
    /// 체력이 0이 됐는지 확인
    /// </summary>
    /// <returns>체력이 0이면 true, 아니면 false</returns>
    public bool isZero();
    /// <summary>
    /// 사망 시 호출하는 함수
    /// </summary>
    public void Dead();


}
