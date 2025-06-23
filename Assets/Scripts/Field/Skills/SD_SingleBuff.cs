using UnityEngine;

[CreateAssetMenu(fileName = "SingleBuff", menuName = "Scriptable Objects/Skill/SingleBuff")]
public class SD_SingleBuff : BaseSkillData
{
    public BuffData buffData;
    public int count;

    public override IActive CreateInstance()
    {
        var instance = new S_SingleBuff(this);
        return instance;
    }
}
