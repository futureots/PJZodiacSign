using System;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    [SerializeField] int _curEnergy;
    public int CurEnergy
    {
        get { return _curEnergy; }
        set
        {
            _curEnergy = value;
            onEnergyChanged?.Invoke(_curEnergy, SkillCost);
        }
    }
    /// <summary>스킬 비용</summary>
    [SerializeField] int _skillCost;
    public int SkillCost
    {
        get { return _skillCost; }
        set
        {
            _skillCost = value;
            onEnergyChanged?.Invoke(CurEnergy, _skillCost);
        }
    }
    public Action<int, int> onEnergyChanged;

    public void RegenerateEnergy()
    {
        CurEnergy = Mathf.Min(CurEnergy + 1, SkillCost);
    }
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;
    /// <summary>
    /// 기물의 스킬 설정
    /// </summary>
    /// <param name="skillData">기물 스킬 데이터</param>
    public void SetupSkill(BaseSkillData skillData, int skillCost = 0)
    {
        this.skillData = skillData;
        _skillCost = skillCost;
    }
    /// <summary>skillData의 인스턴스</summary>
    public IActive GetSkillInstance()
    {
        if (skillData == null) return null;
        var skillInstance = skillData.CreateInstance();
        skillInstance.AddCallback(x => {
            if (x)
            {
                CurEnergy = 0;
            }
        });
        if (skillInstance is IOwnable entitySkill)
        {
            entitySkill.Owner = GetComponent<Entity>();
        }
        return skillInstance;
    }
}
