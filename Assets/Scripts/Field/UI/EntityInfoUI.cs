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
            selectedEntity.OnLevelChanged -= UpdateLevelText;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        UpdateLevelText(selectedEntity.Level);
        selectedEntity.OnLevelChanged += UpdateLevelText;

        hpBar.SetGauge(entity.CurHp, entity.MaxHp);
        entity.OnHpChanged += hpBar.SetGauge;

        energyBar.SetGauge(entity.CurEnergy, entity.SkillCost);
        entity.OnEnergyChanged += energyBar.SetGauge;

        SetPowerText(entity.Power);
        entity.OnPowerChanged += SetPowerText;

        skillInfo.SetSkillUI(entity.skillData);
        buffList.SetBuffUI(entity.buffList);

        bool isSkillUsable = false;
        entitySkillBtn.onClick.RemoveAllListeners();
        // 스킬 버튼 활성화
        if (PhaseManager.curPhase == PhaseType.Battle)
        {
            if (team.isAlly(entity.team))
            {
                Debug.Log(team.teamNumber + " : " + entity.team.teamNumber);
                if (entity.CurEnergy >= entity.SkillCost)
                {
                    isSkillUsable = true;
                }
                entitySkillBtn.onClick.AddListener(() =>
                {
                    transform.root.GetComponent<InputManager>().SetInputMode(entity.skillInstance);
                });
            }

        }

        if (isSkillUsable)
        {
            entitySkillBtn.interactable = true;
        }
        else
        {
            entitySkillBtn.interactable = false;
        }
    }
    public void UpdateLevelText(int level)
    {
        entityName.text = selectedEntity.id + (level == 0 ? "" : $" + {level}");
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
