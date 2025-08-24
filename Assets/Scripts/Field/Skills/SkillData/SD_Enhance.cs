using UnityEngine;

[CreateAssetMenu(fileName = "Enhance", menuName = "Scriptable Objects/SkillData/Enhance")]
public class SD_Enhance : BaseSkillData
{
    public override IActive CreateInstance()
    {
        return new S_Enhance(this);
    }
}
