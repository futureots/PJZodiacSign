using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoPanel : MonoBehaviour
{
    
    public SkillButton entitytSkillButton;
    // Start is called before the first frame update
    void Start()
    {
        entitytSkillButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowPanel(Entity entity)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = entity.name;
        entitytSkillButton.SetSkill(entity.entitySkill);
        if (entity.CompareTag("Player"))
        {
            entitytSkillButton.gameObject.SetActive(true);
        }
        else
        {
            entitytSkillButton.gameObject.SetActive(false);
        }
        Debug.Log(entity.name);
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
