using UnityEngine;

[CreateAssetMenu(fileName = "Enhence", menuName = "Scriptable Objects/SkillData/Enhence")]
public class SD_Enhence : BaseSkillData
{
    public override IActive CreateInstance()
    {
        return new S_Enhence(this);
    }
}
