using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoPanel : MonoBehaviour
{
    public GameObject InfoPanel;
    
    public SkillButton entitytSkillButton;
    // Start is called before the first frame update
    void Start()
    {
        entitytSkillButton.gameObject.SetActive(false);
        InfoPanel.SetActive(false);
    }

    public void ShowPanel(Entity entity)
    {
        InfoPanel.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = entity.name;
        if (entity.CompareTag("Player"))
        {
            entitytSkillButton.gameObject.SetActive(true);
            entitytSkillButton.SetSkill(entity.entitySkill);
        }
        else
        {
            entitytSkillButton.gameObject.SetActive(false);
        }
        Debug.Log(entity.name);
    }
    public void HidePanel()
    {
        InfoPanel.SetActive(false);
    }
}
