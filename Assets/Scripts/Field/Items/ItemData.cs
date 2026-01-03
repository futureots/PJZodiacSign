using Battle.Phase;

public class ItemData : AbstractData
{
    public string description;

    public BaseSkillData skillData;

    /// <summary>
    /// 해당 아이템이 사용가능한 페이즈 타입
    /// </summary>
    public PhaseType useType;

}
