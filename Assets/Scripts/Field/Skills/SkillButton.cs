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
        this.skill = skill;
    }
    protected override void Start()
    {
        base.Start();
        SetSkill(GetComponent<Skill>());
        onClick.AddListener(OnPointerClick);
    }

    private void Update()
    {
        if(skill == null || InputManager.Instance.currentMode == InputManager.Mode.None)
        {
            interactable = false;
        }
        else
        {
            interactable = true;
        }
    }
    public void OnPointerClick()
    {
        InputManager.Instance.SetInputMode(skill);
    }

}
