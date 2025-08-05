using UnityEngine;

[CreateAssetMenu(fileName = "StaleMate", menuName = "Scriptable Objects/SkillData/StaleMate")]
public class SD_AddBuff : BaseSkillData
{
    public BuffData BuffData;
    public int count;
    public override IActive CreateInstance()
    {
        var skill = new S_AddBuff(this);
        return skill;
    }
}
