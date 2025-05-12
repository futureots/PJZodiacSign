using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : Button
{
    public ISkill skill;
    
    public void SetSkill(ISkill skill)
    {
        this.skill = skill;
        
    }
    protected override void Start()
    {
        base.Start();
        skill = GetComponent<ISkill>();
        onClick.AddListener(OnPointerClick);
        
    }

    private void Update()
    {
        if(skill == null || InputManager.Instance.currentMode == InputManager.Mode.None)
        {
            interactable = false;
        }
    }
    public void OnPointerClick()
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
