using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoPanel : MonoBehaviour
{
    Entity selectedEntity;
    public GameObject InfoPanel;
    
    public Button entitySkillBtn;
    public Team team;
    // Start is called before the first frame update
    void Start()
    {
        //entitySkillBtn.gameObject.SetActive(false);
        InfoPanel.SetActive(false);
        team = transform.root.GetComponent<Team>();
    }

    public void ShowPanel(Entity entity)
    {
        selectedEntity = entity;
        InfoPanel.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = entity.id;
        if (team.isAlly(entity.team))
        {
            // 스킬 버튼 활성화
            entitySkillBtn.gameObject.SetActive(true);
            entitySkillBtn.onClick.RemoveAllListeners();
            entitySkillBtn.onClick.AddListener(() =>
            {
                if(entity.curEnergy > entity.skillCost)
                {
                    transform.root.GetComponent<InputManager>().SetInputMode(entity.skillInstance);
                }
            });
            //entitytSkillButton.SetSkill(entity.skillInstance);
        }
        else
        {
            // 스킬 버튼 비활성화(스킬 아이콘을 통해 스킬 설명을 확인할 수 있음)
            entitySkillBtn.gameObject.SetActive(false);
        }
        Debug.Log(entity.name);
    }
    public void HidePanel()
    {
        InfoPanel.SetActive(false);
    }
}
