using System;
using System.Collections;

public interface IAttackable
{
    public int Power { get; set; }
    
    public event Action<int> OnPowerChanged ;
    
    /// <summary>
    /// 공격 시전
    /// </summary>
    public IEnumerator Attack();
}
