using System;
using System.Collections;

public interface IAttackable
{
    public static readonly int Scale = 100;
        
    public int Power { get; set; }
    
    public event Action<int> OnPowerChanged ;
    
    /// <summary>
    /// 공격 시전
    /// </summary>
    public IEnumerator Attack(int multiplier = 100);
}
