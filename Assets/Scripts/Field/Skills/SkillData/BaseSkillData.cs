using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;
    // 스킬에 필요한 입력 조건
    // 실제 스킬이 실행될 때 발동할 이펙트 및 오브젝트
    public List<GameObject> effects;

    public abstract IActive CreateInstance();
}