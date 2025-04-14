using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public Skill skill;

    #region Debugging
    private void Start()
    {
        skill = GetComponent<Skill>();
    }
    #endregion
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InputManager.Instance.selectedSkill == skill
            && skill != null)
        {
            InputManager.Instance.SetSkill(null);
            skill.Reinitialize();
        }
        else
        {
            InputManager.Instance.AllocateSkillCommand(skill);
        }
    }

}
