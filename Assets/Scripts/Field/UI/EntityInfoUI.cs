using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

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
        if (selectedEntity != null)
        {
            selectedEntity.OnHpChanged -= hpBar.SetGauge;
            selectedEntity.OnEnergyChanged -= energyBar.SetGauge;
            selectedEntity.OnPowerChanged -= SetPowerText;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        entityName.text = entity.id + (entity.level == 0 ? "" : $" + {entity.level}");
        hpBar.SetGauge(entity.CurHp, entity.MaxHp);
        entity.OnHpChanged += hpBar.SetGauge;
        energyBar.SetGauge(entity.CurEnergy, entity.SkillCost);
        entity.OnEnergyChanged += energyBar.SetGauge;
        SetPowerText(entity.Power);
        entity.OnPowerChanged += SetPowerText;
        skillInfo.SetSkillUI(entity.skillData);
        buffList.SetBuffUI(entity.buffList);


        // 스킬 버튼 활성화
        if (team.isAlly(entity.team))
        {
            if (entity.CurEnergy >= entity.SkillCost)
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

    void SetPowerText(int value)
    {
        powerText.text = value.ToString();
    }
}
