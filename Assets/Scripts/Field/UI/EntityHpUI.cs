using TMPro;
using UnityEngine;

public class EntityHpUI : MonoBehaviour
{
    Entity _entity;
    EnergyComponent _energy;

    public GameObject levelObject;
    public TextMeshProUGUI levelText;
    
    public GaugeUI hpBar;
    public GaugeUI energyBar;
    private void Start()
    {
        
    }

    // TODO: 컴포넌트 접근 및 구독 방식 개선
    public void SetEntity(Entity entity)
    {
        if(_entity != null)
        {
            _entity.onDead -= OnDead;
            _entity.OnHealthChanged -= hpBar.SetGauge;
            if(_energy !=null) _energy.OnEnergyChanged -= energyBar.SetGauge;
            _entity.OnLevelChanged -= UpdateLevelText;
        }
        _entity = entity;
        

        transform.SetParent(entity.transform);
        transform.localPosition = Vector3.zero + entity.baseData.hpPanelPosition;

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


    }

    private void LateUpdate()
    {
        if (_entity != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
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
    }
}
