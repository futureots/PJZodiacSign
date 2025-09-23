using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] int _curHealth;
    public int CurHealth
    {
        get { return _curHealth; }
        set
        {
            _curHealth = Mathf.Min(value, _maxHealth);
            onHealthChanged?.Invoke(_curHealth, MaxHealth);
        }
    }
    /// <summary>최대 체력</summary>
    [SerializeField] int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
            onHealthChanged?.Invoke(CurHealth, _maxHealth);
        }
    }
    public Action<int, int> onHealthChanged;

    public void Initialize(int maxHp)
    {
        _maxHealth = maxHp;
        _curHealth = maxHp;
    }
}
