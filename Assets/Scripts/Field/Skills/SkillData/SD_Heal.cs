using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/SkillData/Heal")]
public class SD_Heal : BaseSkillData
{
    public override IActive CreateInstance()
    {
        return new S_Heal(this);
    }
}
