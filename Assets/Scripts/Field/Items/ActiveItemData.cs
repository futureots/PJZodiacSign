using UnityEngine;

[CreateAssetMenu(fileName = "ActiveItemData", menuName = "Scriptable Objects/ActiveItemData")]
public class ActiveItemData : ItemData
{
    public BaseSkillData effect;

    public override ItemInstance CreateInstance()
    {
        var instance = new ActiveItemInstance(this);
        // 스킬 데이터로 스킬 인스턴스 제작하기
        instance.effectData = effect;
        return instance;
        
    }
}
