using UnityEngine;

[CreateAssetMenu(fileName = "BaseSkillData", menuName = "Scriptable Objects/SkillData")]
public class BaseSkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public SkillKey skillKey;
    public IActive CreateInstance(Entity owner = null)
    {
        if(owner == null)
        {
            return SkillFactory.CreateInstance(this);
        }
        return SkillFactory.CreateInstance(this, owner);
    }
}