using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damaged(int damage);
    public void Healed(int amount);
    //체력이 0이 됐는지 확인
    public bool isZero();
}
