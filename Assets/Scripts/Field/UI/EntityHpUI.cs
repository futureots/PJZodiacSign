using UnityEngine;
using UnityEngine.UI;

public class EntityHpUI : MonoBehaviour
{
    Entity _entity;

    public Slider hpBar;
    public Slider energyBar;

    public void SetEntity(Entity entity)
    {
        if(_entity != null)
        {
            _entity.OnDead -= SetDead;
            _entity.OnHpChanged -= SethpBar;
            _entity.OnEnergyChanged -= SetEnergyBar;
        }
        _entity = entity;
        

        transform.SetParent(entity.transform);
        transform.localPosition = Vector3.zero + entity.data.hpPanelPosition;


        _entity.OnDead += SetDead;
        _entity.OnHpChanged += SethpBar;
        _entity.OnEnergyChanged += SetEnergyBar;

    }

    private void LateUpdate()
    {
        if (_entity != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
    void SethpBar(int curHp, int maxHp)
    {
        hpBar.value = (float)curHp / maxHp;
    }

    void SetEnergyBar(int curEnergy, int maxEnergy)
    {
        energyBar.value = (float)curEnergy / maxEnergy;
    }
    
    void SetDead()
    {
        Destroy(gameObject);
    }
}
