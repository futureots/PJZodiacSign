using System;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityHpUI : MonoBehaviour
{
    Entity _entity;
    EnergyComponent _energy;

    public GameObject levelObject;
    public TextMeshProUGUI levelText;
    public RectTransform rectTransform;
    
    public GaugeUI hpBar;
    public GaugeUI energyBar;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    // TODO: 컴포넌트 접근 및 구독 방식 개선
    public void SetEntity(Entity entity, Action OnDestroy = null)
    {
        if(_entity != null)
        {
            _entity.onDead -= OnDead;
            _entity.OnHealthChanged -= hpBar.SetGauge;
            if(_energy !=null) _energy.OnEnergyChanged -= energyBar.SetGauge;
            _entity.OnLevelChanged -= UpdateLevelText;
        }
        _entity = entity;


        hpBar.SetGauge(_entity.CurHealth, _entity.MaxHealth);
        _entity.OnHealthChanged += hpBar.SetGauge;

        if(entity.TryGetComponent<EnergyComponent>(out var component))
        {
            _energy = component;
            energyBar.SetGauge(component.CurEnergy, component.MaxEnergy);
            component.OnEnergyChanged += energyBar.SetGauge;
        }
        else
        {
            _energy = null;
        }

        UpdateLevelText(_entity.Level);
        _entity.OnLevelChanged += UpdateLevelText;

        _entity.onDead += OnDead;
        _entity.onDead += OnDestroy;

        _entity.team.OnTeamChanged += (team) => OnTeamChange();
        Agent.OnLocalPlayerChanged += OnTeamChange;

    }

    private void LateUpdate()
    {
        if (_entity != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            Vector3 worldPos = _entity.transform.position + Vector3.Scale(_entity.baseData.offset, _entity.transform.lossyScale);
            //Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            rectTransform.position = worldPos;
        }
    }
    void UpdateLevelText(int level, int prev=0)
    {
        if (level ==0)
        {
            levelObject.SetActive(false);
        }
        else
        {
            levelObject.SetActive(true);
            levelText.text = level.ToString();
        }
            
    }

    void OnTeamChange()
    {
        if (!Agent.LocalPlayer) hpBar.gaugeBar.color = Color.red;
        // TODO : 팀별로 체력바 색상 다르게 표시(다른 곳으로 이전 필요)
        else if (_entity.team.IsAlly(Agent.LocalPlayer.id))
        {
            hpBar.gaugeBar.color = Color.green;
        }
        else
        {
            hpBar.gaugeBar.color = Color.red;
        }
    }
    
    
    void OnDead()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_entity != null)
        {
            _entity.onDead -= OnDead;
            _entity.OnHealthChanged -= hpBar.SetGauge;
            if(_energy !=null) _energy.OnEnergyChanged -= energyBar.SetGauge;
            _entity.OnLevelChanged -= UpdateLevelText;
        }
        Agent.OnLocalPlayerChanged -= OnTeamChange;
    }
}
