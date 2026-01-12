using Battle.Phase;
using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : MonoBehaviour
{
    IPhaseManageService _service;
    InputManager _inputManager;

    Entity selectedEntity;
    SkillComponent _skill;
    EnergyComponent _energy;
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

    public void Init(InputManager input,IPhaseManageService service)
    {
        _service = service;
        _inputManager = input;
        team = _inputManager.GetComponent<Team>();
    }

    public void ShowPanel(Entity entity)
    {
        if (selectedEntity != null)
        {
            selectedEntity.health.onHealthChanged -= hpBar.SetGauge;
            selectedEntity.OnLevelChanged -= UpdateLevelText;

            if (_power != null) _power.onPowerChanged -= SetPowerText;
            if(_energy !=null) _energy.onEnergyChanged -= energyBar.SetGauge;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        UpdateLevelText(selectedEntity.Level);
        selectedEntity.OnLevelChanged += UpdateLevelText;

        hpBar.SetGauge(entity.health.CurHealth, entity.health.MaxHealth);
        entity.health.onHealthChanged += hpBar.SetGauge;

        // 공격력 표시
        if(entity.TryGetComponent<PowerComponent>(out var power))
        {
            powerText.gameObject.SetActive(true);
            _power = power;
            Debug.Log("PowerComponent : " + power.Power);
            SetPowerText(power.Power);
            power.onPowerChanged += SetPowerText;
        }
        else
        {
            powerText.gameObject.SetActive(false);
        }

        // 버프 표시
        if (entity.TryGetComponent<BuffManager>(out var buffs))
        {
            buffList.SetBuffUI(buffs);
        }

        if(entity.TryGetComponent<EnergyComponent>(out var energy))
        {
            _energy = energy;
            energyBar.SetGauge(_energy.CurEnergy, _energy.MaxEnergy);
            _energy.onEnergyChanged += energyBar.SetGauge;
        }

        // 스킬 및 마나 표시
        if (entity.TryGetComponent<SkillComponent>(out var skill))
        {
            _skill = skill;

            skillInfo.SetSkillUI(_skill.skillData);

            bool isSkillUsable = false;
            entitySkillBtn.onClick.RemoveAllListeners();
            // 스킬 버튼 활성화
            if (_service.CurPhase == PhaseType.Battle && _inputManager.curModeState is MoveModeInput)
            {
                if (team.IsAlly(entity.team))
                {
                    if (_skill.skillData != null)
                    {
                        // 스킬의 조건을 만족했는지 확인하는 조건문
                        if (_skill.IsUsable())
                        {
                            isSkillUsable = true;
                        }
                        entitySkillBtn.onClick.AddListener(() =>
                        {
                            // TODO : 스킬 입력 모드로 변경 및 입력에 필요한 값 전송
                            _inputManager.SetInputMode(skill);
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
    public void UpdateLevelText(int level, int prevLevel = 0)
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
