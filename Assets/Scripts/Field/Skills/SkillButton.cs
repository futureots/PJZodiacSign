using System.Collections;
using System.Collections.Generic;
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
    #region Debugging
    protected override void Start()
    {
        base.Start();
        skill = GetComponent<ISkill>();
        onClick.AddListener(OnPointerClick);
    }
    #endregion
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
