using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : MonoBehaviour
{
    Entity selectedEntity;

    [Header("정보 UI")]
    public GameObject InfoPanel;
    public TextMeshProUGUI entityName;
    public GaugeUI hpBar;
    public GaugeUI energyBar;
    // 스킬 세팅 용 컴포넌트 제작
    // 버프 리스트 표시용 컴포넌트 제작
    // 레벨 표시 컴포넌트 제작
    
    public Button entitySkillBtn;
    Team team;
    void Start()
    {
        InfoPanel.SetActive(false);
        team = transform.root.GetComponent<Team>();
    }

    public void ShowPanel(Entity entity)
    {
        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        entityName.text = entity.id;

        hpBar.SetGauge(entity.curHp, entity.maxHp);
        energyBar.SetGauge(entity.curEnergy, entity.skillCost);

        // 스킬 버튼 활성화
        if (team.isAlly(entity.team))
        {
            entitySkillBtn.interactable = true;
            entitySkillBtn.onClick.RemoveAllListeners();
            entitySkillBtn.onClick.AddListener(() =>
            {
                if(entity.curEnergy >= entity.skillCost)
                {
                    transform.root.GetComponent<InputManager>().SetInputMode(entity.skillInstance);
                }
            });
        }
        else
        {
            // 스킬 버튼 비활성화(스킬 아이콘을 통해 스킬 설명을 확인할 수 있음)
            entitySkillBtn.interactable = false;
        }
    }
    public void HidePanel()
    {
        InfoPanel.SetActive(false);
    }
}
