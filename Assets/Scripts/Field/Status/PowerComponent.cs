using System;
using UnityEngine;

// TODO: DEPRECATED
public class PowerComponent : MonoBehaviour
{
    /// <summary>공격력</summary>
    [SerializeField] int _power;
    public int Power
    {
        get { return _power; }
        set
        {
            _power = value;
            OnPowerChanged?.Invoke(_power);
        }
    }
    public Action<int> OnPowerChanged;

    public void Init(int power)
    {
        _power = power;
    }
}
