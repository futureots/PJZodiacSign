using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHpUI : MonoBehaviour
{
    Entity _entity;

    public GameObject levelObject;
    public TextMeshProUGUI levelText;
    
    public GaugeUI hpBar;
    public GaugeUI energyBar;
    private void Start()
    {
        Entity.onDead += OnDead;
    }

    public void SetEntity(Entity entity)
    {
        if(_entity != null)
        {
            _entity.onHpChanged -= hpBar.SetGauge;
            _entity.onEnergyChanged -= energyBar.SetGauge;
            _entity.onLevelChanged -= UpdateLevelText;
        }
        _entity = entity;
        

        transform.SetParent(entity.transform);
        transform.localPosition = Vector3.zero + entity.data.hpPanelPosition;

        hpBar.SetGauge(_entity.CurHp, _entity.MaxHp);
        _entity.onHpChanged += hpBar.SetGauge;

        energyBar.SetGauge(_entity.CurEnergy, _entity.SkillCost);
        _entity.onEnergyChanged += energyBar.SetGauge;

        UpdateLevelText(_entity.Level);
        _entity.onLevelChanged += UpdateLevelText;

        
        

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
    
    void OnDead(Entity entity)
    {
        if(entity == _entity)
        {
            Destroy(gameObject);
        }
            
    }
    private void OnDestroy()
    {
        Entity.onDead -= OnDead;
    }
}
