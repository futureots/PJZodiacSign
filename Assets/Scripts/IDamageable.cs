using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    
    public void Damaged(int damage,ColorType type = ColorType.Empty);
    public void Healed(int amount);
    //체력이 0이 됐는지 확인
    public bool isZero();
    //체력이 0이 되면 오브젝트 파괴 함수
    public void Dead();
}
