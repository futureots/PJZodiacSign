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
    public TextMeshProUGUI powerText;
    public BuffListUI buffList;
    public SkillInfoUI skillInfo;
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
        entityName.text = entity.id + (entity.level == 0 ? "" : $" + {entity.level}");
        hpBar.SetGauge(entity.curHp, entity.maxHp);
        energyBar.SetGauge(entity.curEnergy, entity.skillCost);
        powerText.text = entity.power.ToString();
        skillInfo.SetSkillUI(entity.skillData);
        buffList.SetBuffUI(entity.buffList);


        // 스킬 버튼 활성화
        if (team.isAlly(entity.team))
        {
            if (entity.curEnergy >= entity.skillCost)
            {
                entitySkillBtn.interactable = true;
            }
            entitySkillBtn.onClick.RemoveAllListeners();
            entitySkillBtn.onClick.AddListener(() =>
            {
                transform.root.GetComponent<InputManager>().SetInputMode(entity.skillInstance);
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
