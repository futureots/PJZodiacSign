using System;
using System.Collections;

[System.Serializable]
public class ItemComponent : SkillComponent
{
    public ItemData itemData { get; private set; }

    public Action OnDiscard;

    public void Init(ItemData itemData)
    {
        this.itemData = itemData;
        skillData = itemData.skillData;
        skillLogic = skillData.skillLogic.Clone();
        skillLogic.SetSkillComponent(this);
    }

    public override bool IsUsable()
    {
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        yield return StartCoroutine(skillLogic.ExecuteSkill());
        Discard();
    }

    /// <summary>
    /// 아이템 버리기
    /// </summary>
    public void Discard()
    {
        OnDiscard?.Invoke();
    }



}
