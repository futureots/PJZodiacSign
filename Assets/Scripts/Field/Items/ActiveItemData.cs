using UnityEngine;

[CreateAssetMenu(fileName = "ActiveItemData", menuName = "Scriptable Objects/ActiveItemData")]
public class ActiveItemData : ItemData
{
    public BaseSkillData effect;


    public override ItemInstance CreateInstance()
    {
        var instance = new ActiveItemInstance(this);
        
        instance.effectData = effect;
        return instance;
        
    }
}
