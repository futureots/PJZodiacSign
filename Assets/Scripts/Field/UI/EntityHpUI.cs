using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityHpUI : MonoBehaviour
{
    Entity _entity;
    SkillComponent _skill;

    public GameObject levelObject;
    public TextMeshProUGUI levelText;
    
    public GaugeUI hpBar;
    public GaugeUI energyBar;
    private void Start()
    {
        
    }

    public void SetEntity(Entity entity)
    {
        if(_entity != null)
        {
            _entity.onDead -= OnDead;
            _entity.onHpChanged -= hpBar.SetGauge;
            if(_skill !=null) _skill.onEnergyChanged -= energyBar.SetGauge;
            _entity.onLevelChanged -= UpdateLevelText;
        }
        _entity = entity;
        

        transform.SetParent(entity.transform);
        transform.localPosition = Vector3.zero + entity.baseData.hpPanelPosition;

        hpBar.SetGauge(_entity.CurHp, _entity.MaxHp);
        _entity.onHpChanged += hpBar.SetGauge;

        if(entity.TryGetComponent<SkillComponent>(out var component))
        {
            _skill = component;
            energyBar.SetGauge(component.CurEnergy, component.SkillCost);
            component.onEnergyChanged += energyBar.SetGauge;
        }
        else
        {
            _skill = null;
        }

            UpdateLevelText(_entity.Level);
        _entity.onLevelChanged += UpdateLevelText;

        _entity.onDead += OnDead;


    }

    private void LateUpdate()
    {
        if (_entity != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
    void UpdateLevelText(int level)
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
            _entity.onHpChanged -= hpBar.SetGauge;
            if(_skill !=null) _skill.onEnergyChanged -= energyBar.SetGauge;
            _entity.onLevelChanged -= UpdateLevelText;
        }
    }
}
