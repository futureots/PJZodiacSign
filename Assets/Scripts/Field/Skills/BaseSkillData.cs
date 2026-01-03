using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;
    // 스킬에 필요한 입력 조건

    [SerializeReference, SubclassSelector]
    public BaseSkillLogic skillLogic;
}