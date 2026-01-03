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
    }

    public override bool IsUsable()
    {
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        // TODO : 자식 클래스 내에 저장된 변수를 사용해 각 스킬의 로직을 코루틴으로 구현
        yield return null;
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
