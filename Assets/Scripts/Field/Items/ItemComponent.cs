using System;
using System.Collections;

[Serializable]
public class ItemComponent : SkillComponent
{
    public ItemData ItemData { get; private set; }

    public Action onDiscard;

    private bool _isUsed;
    public void Init(ItemData itemData)
    {
        this.ItemData = itemData;
        skillData = itemData.skillData;
        skillLogic = skillData.skillLogic.Clone();
        skillLogic.SetSkillComponent(this);
        _isUsed = false;
    }

    
    public override bool IsUsable()
    {
        return !_isUsed;
    }

    public override IEnumerator ExecuteSkill()
    {
        _isUsed = true;
        yield return skillLogic.ExecuteSkill();
        
        Discard();
        
    }

    /// <summary>
    /// 아이템 버리기
    /// </summary>
    public void Discard()
    {
        onDiscard?.Invoke();
    }



}
