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
    PowerComponent _power;

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
            selectedEntity.onLevelChanged -= UpdateLevelText;

            if (_power != null) _power.onPowerChanged -= SetPowerText;
            if(_skill !=null) _skill.onEnergyChanged -= energyBar.SetGauge;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        UpdateLevelText(selectedEntity.Level);
        selectedEntity.onLevelChanged += UpdateLevelText;

        hpBar.SetGauge(entity.CurHp, entity.MaxHp);
        entity.onHpChanged += hpBar.SetGauge;

        // 공격력 표시
        if(entity.TryGetComponent<PowerComponent>(out var power))
        {
            _power = power;
            Debug.Log("PowerComponent : " + power.Power);
            SetPowerText(power.Power);
            power.onPowerChanged += SetPowerText;
        }

        // 버프 표시
        if (entity.TryGetComponent<BuffManager>(out var buffs))
        {
            buffList.SetBuffUI(buffs);
        }

        // 스킬 및 마나 표시
        if (entity.TryGetComponent<SkillComponent>(out var skill))
        {
            _skill = skill;

            energyBar.SetGauge(_skill.CurEnergy, _skill.SkillCost);
            _skill.onEnergyChanged += energyBar.SetGauge;

            skillInfo.SetSkillUI(_skill.skillData);

            bool isSkillUsable = false;
            entitySkillBtn.onClick.RemoveAllListeners();
            // 스킬 버튼 활성화
            if (PhaseManager.curPhase == PhaseType.Battle)
            {
                if (team.IsAlly(entity.team))
                {
                    if (_skill.skillData != null)
                    {
                        if (_skill.CurEnergy >= _skill.SkillCost)
                        {
                            isSkillUsable = true;
                        }
                        entitySkillBtn.onClick.AddListener(() =>
                        {
                            transform.root.GetComponent<InputManager>().SetInputMode(_skill.GetSkillInstance());
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
