using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public Skill skill;
    public void OnPointerClick(PointerEventData eventData)
    {
        ref var curSkill = ref InputManager.Instance.selectedSkill;
        if (curSkill == skill)
        {
            curSkill = null;
        }
        else
        {
            curSkill = skill;
            InputManager.Instance.AllocateSkillCommand(skill);
        }
            

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
