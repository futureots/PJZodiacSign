using System;
using System.Collections;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;
    public BaseSkillLogic skillLogic;
    public int silenceCount;
    bool isSilenced
    {
        get
        {
            return silenceCount > 0;
        }
    }

    public void Init(BaseSkillData data)
    {
        skillData = data;
        skillLogic = data.skillLogic.Clone();
    }

    // 스킬 사용이 가능한지 반환하는 함수
    public virtual bool IsUsable()
    {
        if (!isSilenced)
        {
            if (TryGetComponent<EnergyComponent>(out var energy))
            {
                return energy.CurEnergy >= energy.MaxEnergy;
            }
        }
        return false;
    }

    public virtual IEnumerator ExecuteSkill()
    {
        // TODO : 자식 클래스 내에 저장된 변수를 사용해 각 스킬의 로직을 코루틴으로 구현
        yield return null;
        if (TryGetComponent<EnergyComponent>(out var energy))
        {
            energy.CurEnergy = 0;
        }
    }

}