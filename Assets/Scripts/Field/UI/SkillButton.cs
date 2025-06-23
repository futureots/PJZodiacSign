using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{

    IActive skill;
    
    public void SetSkill(IActive skill)
    {
        Debug.Log($"SetSkill + {skill}");
        this.skill = skill;
        if (skill != null)
        {
            //interactable = true;
        }
        else
        {
            //interactable = false;
        }
        //skill.callback += 
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!InputManager.isInputStop)
        {
            InputManager.Instance.SetInputMode(skill);
        }
    }
}
