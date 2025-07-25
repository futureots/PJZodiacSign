using UnityEngine;

[CreateAssetMenu(fileName = "StaleMate", menuName = "Scriptable Objects/SkillData/StaleMate")]
public class SD_StaleMate : BaseSkillData
{
    public override IActive CreateInstance()
    {
        var skill = new S_StaleMate(this);
        return skill;
    }
}
