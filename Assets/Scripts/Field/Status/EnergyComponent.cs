using System;
using UnityEngine;

public class EnergyComponent : MonoBehaviour
{
    [SerializeField] int _curEnergy;
    public int CurEnergy
    {
        get { return _curEnergy; }
        set
        {
            _curEnergy = value;
            OnEnergyChanged?.Invoke(_curEnergy, MaxEnergy);
        }
    }
    /// <summary>스킬 비용</summary>
    [SerializeField] int _maxEnergy;
    public int MaxEnergy
    {
        get { return _maxEnergy; }
        set
        {
            _maxEnergy = value;
            OnEnergyChanged?.Invoke(CurEnergy, _maxEnergy);
        }
    }
    public Action<int, int> OnEnergyChanged;

    public void Initialize(int skillCost)
    {
        _maxEnergy = skillCost;
        _curEnergy = 0;
    }

    public void RegenerateEnergy()
    {
        CurEnergy = Mathf.Min(CurEnergy + 1, MaxEnergy);
    }
}
