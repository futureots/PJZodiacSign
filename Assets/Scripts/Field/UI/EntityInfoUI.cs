using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : MonoBehaviour
{
    InputManager _inputManager;
    int teamId;

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

    void Start()
    {
        InfoPanel.SetActive(false);
    }

    public void Init(InputManager input)
    {
        _inputManager = input;
        teamId = _inputManager.agent.id;
        input.OnObjectClicked.AddListener(OnObjectClick);
        input.onModeChanged += SetSkillButton;
        entitySkillBtn.onClick.AddListener(UseSkill);
    }

    void OnObjectClick(GameObject obj)
    {
        if (obj.TryGetComponent<Entity>(out var entity))
        {
            ShowPanel(entity);
        }
        else
        {
            HidePanel();
        }
    }

    /// <summary>
    /// 기물의 정보 출력
    /// </summary>
    /// <param name="entity"></param>
    public void ShowPanel(Entity entity)
    {
        if (selectedEntity != null)
        {
            selectedEntity.health.onHealthChanged -= hpBar.SetGauge;
            selectedEntity.onLevelChanged -= SetLevelText;

            if (_power != null) _power.onPowerChanged -= SetPowerText;
            if(_energy !=null) _energy.onEnergyChanged -= energyBar.SetGauge;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        SetLevelText(selectedEntity.Level);
        selectedEntity.onLevelChanged += SetLevelText;

        hpBar.SetGauge(entity.health.CurHealth, entity.health.MaxHealth);
        entity.health.onHealthChanged += hpBar.SetGauge;

        // 공격력 표시
        if(entity.TryGetComponent<PowerComponent>(out var power))
        {
            powerText.gameObject.SetActive(true);
            _power = power;
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

        // 에너지 표시
        if(entity.TryGetComponent<EnergyComponent>(out var energy))
        {
            _energy = energy;
            energyBar.SetGauge(_energy.CurEnergy, _energy.MaxEnergy);
            _energy.onEnergyChanged += energyBar.SetGauge;
        }

        // 스킬 표시
        if (entity.TryGetComponent<SkillComponent>(out var skill))
        {
            _skill = skill;
            skillInfo.SetSkillUI(_skill.skillData);
            SetSkillButton(_inputManager.curModeState);
        }
        else
        {
            entitySkillBtn.interactable = false;
        }
    }
    void HidePanel()
    {
        InfoPanel.SetActive(false);
    }
    void SetLevelText(int level, int prevLevel = 0)
    {
        entityName.text = selectedEntity.baseData.productName + (level == 0 ? "" : $" + {level}");
    }
    void SetPowerText(int value)
    {
        powerText.text = value.ToString();
    }
    void UseSkill()
    {
        if(_skill != null)
            _inputManager.SetInputMode(_skill);
    }
    void SetSkillButton(IInputState state)
    {
        bool isUsable = false;
        if (state is MoveModeInput)
        {
            if(selectedEntity != null)
            {
                if (selectedEntity.team.IsAlly(teamId))
                {
                    if (_skill.IsUsable())
                    {
                        isUsable = true;
                    }
                }
            }
        }
        if (isUsable)
        {
            entitySkillBtn.interactable = true;
        }
        else
        {
            entitySkillBtn.interactable = false;
        }

    }
}
