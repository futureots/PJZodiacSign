using System.Reflection;
using UnityEngine;

public class ActiveItemInstance : ItemInstance, IUsable
{
    public BaseSkillData effectData;
    public ActiveItemInstance(ItemData itemData) : base(itemData) { }


    public IActive GetUseEffect()
    {
        var effect = effectData.CreateInstance();
        return effect;
    }
}