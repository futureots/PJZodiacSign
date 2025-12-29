using System.Collections;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;

    public int silenceCount;
    bool isSilenced
    {
        get
        {
            return silenceCount > 0;
        }
    }

    // 스킬 사용이 가능한지 반환하는 함수
    public bool IsUsable()
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

    #region oldSkill
    /// <summary>
    /// 기물의 스킬 설정
    /// </summary>
    /// <param name="skillData">기물 스킬 데이터</param>
    //public void SetupSkill(BaseSkillData skillData)
    //{
    //    this.skillData = skillData;

    //}
    /// <summary>skillData의 인스턴스</summary>
    //public IActive GetSkillInstance()
    //{
    //    if (skillData == null) return null;
    //    var skillInstance = skillData.CreateInstance();
        
    //    // 스킬 사용 성공 시 콜백함수 설정하기
    //    skillInstance.AddCallback(x => {
    //        if (x)
    //        {
    //            if(TryGetComponent<EnergyComponent>(out var energy))
    //            {
    //                energy.CurEnergy = 0;
    //            }
    //        }
    //    });
    //    if (skillInstance is IOwnable entitySkill)
    //    {
    //        entitySkill.Owner = GetComponent<Entity>();
    //    }
    //    return skillInstance;
    //}
    #endregion

    #region new skillSystem

    //public event Action<bool> onSkillSuccess;
    public virtual IEnumerator InputSkill()
    {
        // TODO : skillData에 있는 로직을 불러와 입력 코루틴 호출
        yield return null;
    }
    public virtual IEnumerator ExecuteSkill()
    {
        // TODO : 자식 클래스 내에 저장된 변수를 사용해 각 스킬의 로직을 코루틴으로 구현
        yield return null;
    }

    #endregion
}
