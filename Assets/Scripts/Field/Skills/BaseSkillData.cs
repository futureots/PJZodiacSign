using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    [TextArea(3,5)]
    public string skillDescription;
    // 스킬에 필요한 입력 조건

    [SerializeReference, SubclassSelector]
    public BaseSkillLogic skillLogic;
}