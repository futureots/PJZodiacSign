using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Transform target;
    Health hpComponent;
    Slider hpSlider;
    public void SetHpBar(GameObject obj)
    {
        target = obj.transform;
        hpComponent = obj.GetComponent<Health>();
        hpComponent.healthChanged += UpdateHpBar;
        UpdateHpBar();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
    public void UpdateHpBar(int amount=0)
    {
        if(hpSlider == null)
        {
            hpSlider = GetComponentInChildren<Slider>();
        }
        if (hpSlider == null) return;
        if (hpComponent.maxHp == 0)
        {
            hpSlider.value = 0;
        }
        else
        {
            hpSlider.value = (float)hpComponent.hp / hpComponent.maxHp;
        }

    }
}
