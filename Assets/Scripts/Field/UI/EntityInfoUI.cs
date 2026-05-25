using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : InputManagerUI
{
    [SerializeField] InputManager inputManager;
    PlayerID id;

    Entity selectedEntity;
    SkillComponent _skill;
    EnergyComponent _energy;

    [Header("정보 UI")]
    public GameObject InfoPanel;
    public TextMeshProUGUI entityName;
    public GaugeUI hpBar;
    public GaugeUI energyBar;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI defText;
    public SkillInfoUI skillInfo;
    public Button entitySkillBtn;


    void Start()
    {
        InfoPanel.SetActive(false);
    }

    public  override void Init(InputManager input)
    {
        inputManager = input;
        id = inputManager.agent.id;
        input.OnObjectClicked.AddListener(OnObjectClick);
        input.onModeChanged += SetSkillButton;
        entitySkillBtn.onClick.AddListener(UseSkill);
    }

    void OnObjectClick(GameObject obj)
    {
        if (!obj) return;
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
            selectedEntity.OnHealthChanged -= hpBar.SetGauge;
            selectedEntity.OnLevelChanged -= SetLevelText;

            entity.OnPowerChanged -= SetPowerText;
            if(_energy !=null) _energy.OnEnergyChanged -= energyBar.SetGauge;
        }

        selectedEntity = entity;
        InfoPanel.SetActive(true);
        // 정보 표시
        SetLevelText(selectedEntity.Level);
        selectedEntity.OnLevelChanged += SetLevelText;

        hpBar.SetGauge(entity.CurHealth, entity.MaxHealth);
        entity.OnHealthChanged += hpBar.SetGauge;

        // 공격력 표시
        SetPowerText(entity.Power);
        entity.OnPowerChanged += SetPowerText;
        
        SetDefText(entity.Defense);
        entity.OnDefenseChanged += SetDefText;
        
        if (entity.TryGetComponent<EnergyComponent>(out var energy))
        {
            _energy = energy;
            energyBar.SetGauge(_energy.CurEnergy, _energy.MaxEnergy);
            _energy.OnEnergyChanged += energyBar.SetGauge;
        }

        // 스킬 표시
        if (entity.TryGetComponent<SkillComponent>(out var skill))
        {
            _skill = skill;
            skillInfo.SetSkillUI(_skill.skillData);
            SetSkillButton(inputManager.curModeState);
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
    void SetDefText(int value)
    {
        defText.text = value.ToString();
    }
    void UseSkill()
    {
        if(_skill != null)
            inputManager.SetInputMode(_skill);
        HidePanel();
    }
    void SetSkillButton(IInputState state)
    {
        bool isUsable = false;
        if (state is MoveModeInput)
        {
            if(selectedEntity != null)
            {
                if (selectedEntity.team.IsAlly(id))
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
