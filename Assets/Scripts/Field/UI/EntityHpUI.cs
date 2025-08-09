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


    public void SetEntity(Entity entity)
    {
        if(_entity != null)
        {
            _entity.OnDead -= OnDead;
            _entity.OnHpChanged -= hpBar.SetGauge;
            _entity.OnEnergyChanged -= energyBar.SetGauge;
            _entity.OnLevelChanged -= UpdateLevelText;
        }
        _entity = entity;
        

        transform.SetParent(entity.transform);
        transform.localPosition = Vector3.zero + entity.data.hpPanelPosition;


        _entity.OnDead += OnDead;

        hpBar.SetGauge(_entity.CurHp, _entity.MaxHp);
        _entity.OnHpChanged += hpBar.SetGauge;

        energyBar.SetGauge(_entity.CurEnergy, _entity.SkillCost);
        _entity.OnEnergyChanged += energyBar.SetGauge;

        UpdateLevelText(_entity.Level);
        _entity.OnLevelChanged += UpdateLevelText;

        
        

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
}
