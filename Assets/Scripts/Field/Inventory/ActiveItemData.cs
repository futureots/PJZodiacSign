using UnityEngine;

[CreateAssetMenu(fileName = "ActiveItemData", menuName = "Scriptable Objects/ActiveItemData")]
public class ActiveItemData : ItemData
{
    public AbstractSkillData effect;

    public override ItemInstance CreateInstance()
    {
        var instance = new ActiveItemInstance(this);
        instance.effect = effect.CreateInstance();
        return instance;
        
    }
}
