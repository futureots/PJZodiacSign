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
    private void Awake()
    {
        Init(skillData);
    }
    public void Init(BaseSkillData data)
    {
        if (!data) return;      // No Skill
        
        skillData = data;
        skillLogic = data.skillLogic.Clone();
        skillLogic.SetSkillComponent(this);
    }

    /// <summary>
    /// 스킬 사용 가능 여부 반환
    /// </summary>
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

    /// <summary>
    /// 스킬 실행
    /// </summary>
    public virtual IEnumerator ExecuteSkill()
    {
        // TODO : 자식 클래스 내에 저장된 변수를 사용해 각 스킬의 로직을 코루틴으로 구현
        
        yield return skillLogic.ExecuteSkill();
        if (TryGetComponent<EnergyComponent>(out var energy))
        {
            energy.CurEnergy = 0;
        }
    }

}