using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    ISkill skill;

    #region Debugging
    private void Start()
    {
        
    }
    #endregion
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InputManager.Instance.selectedSkill == skill && skill!=null)
        {
            InputManager.Instance.SetSkill(null);
        }
        else
        {
            skill = new Skill();
            InputManager.Instance.AllocateSkillCommand(skill);
        }
    }

}
