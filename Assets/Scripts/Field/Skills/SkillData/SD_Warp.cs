using UnityEngine;

[CreateAssetMenu(fileName = "Warp", menuName = "Scriptable Objects/SkillData/Warp")]
public class SD_Warp : BaseSkillData
{
    public override IActive CreateInstance()
    {
        var skill = new S_Warp(this);
        return skill;
    }
}
