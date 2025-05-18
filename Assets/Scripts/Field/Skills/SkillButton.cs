using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : Button
{
    
    public Skill skill;
    
    public void SetSkill(Skill skill)
    {
        Debug.Log($"SetSkill + {skill}");
        this.skill = skill;
        if (skill != null)
        {
            interactable = true;
        }
        else
        {
            interactable = false;
        }
    }
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(OnPointerClick);
    }

    public void OnPointerClick()
    {
        if (!InputManager.isInputStop)
        {
            InputManager.Instance.SetInputMode(skill);
        }
    }

}
