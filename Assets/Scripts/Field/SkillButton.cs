using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public Skill skill;
    Skill skillInstance;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InputManager.Instance.selectedSkill == skillInstance && skillInstance != null)
        {
            InputManager.Instance.SetSkill(null);
        }
        else
        {
            skillInstance = Instantiate(skill) as Skill;
            InputManager.Instance.AllocateSkillCommand(skillInstance);
        }
    }

}
