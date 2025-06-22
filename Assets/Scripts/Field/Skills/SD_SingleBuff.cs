using UnityEngine;

[CreateAssetMenu(fileName = "SingleBuff", menuName = "Scriptable Objects/Skill/SingleBuff")]
public class SD_SingleBuff : BaseSkillData<S_SingleBuff>
{
    public BuffData buffData;
    public int count;
}
