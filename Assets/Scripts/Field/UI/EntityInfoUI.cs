using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityInfoUI : MonoBehaviour
{
    Entity selectedEntity;
    SkillComponent _skill;

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
            selectedEntity.onHpChanged -= hpBar.SetGauge;
            selectedEntity.onPowerChanged -= SetPowerText;
            selectedEntity.onLevelChanged -= UpdateLevelText;

            if(_skill !=null) _skill.onEnergyChanged -= energyBar.SetGauge;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        UpdateLevelText(selectedEntity.Level);
        selectedEntity.onLevelChanged += UpdateLevelText;

        hpBar.SetGauge(entity.CurHp, entity.MaxHp);
        entity.onHpChanged += hpBar.SetGauge;

        SetPowerText(entity.Power);
        entity.onPowerChanged += SetPowerText;


        buffList.SetBuffUI(entity.buffList);


        if (TryGetComponent<SkillComponent>(out var skill))
        {
            energyBar.SetGauge(skill.CurEnergy, skill.SkillCost);
            skill.onEnergyChanged += energyBar.SetGauge;

            skillInfo.SetSkillUI(skill.skillData);

            bool isSkillUsable = false;
            entitySkillBtn.onClick.RemoveAllListeners();
            // 스킬 버튼 활성화
            if (PhaseManager.curPhase == PhaseType.Battle)
            {
                if (team.IsAlly(entity.team))
                {
                    if (skill.skillData != null)
                    {
                        if (skill.CurEnergy >= skill.SkillCost)
                        {
                            isSkillUsable = true;
                        }
                        entitySkillBtn.onClick.AddListener(() =>
                        {
                            transform.root.GetComponent<InputManager>().SetInputMode(skill.GetSkillInstance());
                        });
                    }
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
    }
    public void UpdateLevelText(int level)
    {
        entityName.text = selectedEntity.baseData.productName + (level == 0 ? "" : $" + {level}");
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
