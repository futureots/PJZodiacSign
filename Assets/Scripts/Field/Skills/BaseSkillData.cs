using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string SkillName => skillName.GetLocalizedString();
    [SerializeField] private LocalizedString skillName;
    public string SkillDescription => skillDescription.GetLocalizedString();

    [SerializeField] private LocalizedString skillDescription;
    // 스킬에 필요한 입력 조건

    [SerializeReference, SubclassSelector]
    public BaseSkillLogic skillLogic;
}