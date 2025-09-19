using System;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;
    /// <summary>
    /// 기물의 스킬 설정
    /// </summary>
    /// <param name="skillData">기물 스킬 데이터</param>
    public void SetupSkill(BaseSkillData skillData)
    {
        this.skillData = skillData;

    }
    /// <summary>skillData의 인스턴스</summary>
    public IActive GetSkillInstance()
    {
        if (skillData == null) return null;
        var skillInstance = skillData.CreateInstance();
        
        // 스킬 사용 성공 시 콜백함수 설정하기
        skillInstance.AddCallback(x => {
            if (x)
            {
                if(TryGetComponent<EnergyComponent>(out var energy))
                {
                    energy.CurEnergy = 0;
                }
            }
        });
        if (skillInstance is IOwnable entitySkill)
        {
            entitySkill.Owner = GetComponent<Entity>();
        }
        return skillInstance;
    }
}
