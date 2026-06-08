using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsInfoUI : MonoBehaviour
{
    public RectTransform RectTransform { get; private set; }
    [SerializeField] private TextMeshProUGUI goodsDescription;
    [SerializeField] private GameObject areaGroup;
    [SerializeField] private AreaUIBlock moveArea;
    [SerializeField] private AreaUIBlock attackArea;
    [SerializeField] private AreaUIBlock skillArea;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void SetInfo(AbstractData data)
    {
        if (data is EntityData entityData)
        {
            areaGroup.SetActive(true);
            RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 400);
            
            moveArea.obj.SetActive(true);
            moveArea.image.sprite = entityData.moveArea.areaImage;
            
            attackArea.obj.SetActive(true);
            attackArea.image.sprite = entityData.attackArea.areaImage;
            // 스킬이 독자적인 범위를 가질 경우에만 활성화
            if (entityData.skill.skillLogic is AreaSkillLogic areaSkillLogic)
            {
                skillArea.obj.SetActive(true);
                skillArea.image.sprite = areaSkillLogic.Area.areaImage;
            }
            else
            {
                skillArea.obj.SetActive(false);
            }

            goodsDescription.text = entityData.skill.SkillDescription;
        }
        else if (data is ItemData itemData)
        {
            moveArea.obj.SetActive(false);
            attackArea.obj.SetActive(false);
            
            // 스킬이 독자적인 범위를 가질 경우에만 활성화
            if (itemData.skillData.skillLogic is AreaSkillLogic areaSkillLogic)
            {
                areaGroup.SetActive(true);
                RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 400);
                
                skillArea.obj.SetActive(true);
                skillArea.image.sprite = areaSkillLogic.Area.areaImage;
            }
            else
            {
                skillArea.obj.SetActive(false);
                
                areaGroup.SetActive(false);
                RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 200);
            }
            goodsDescription.text = itemData.skillData.SkillDescription;
        }
    }
}

[Serializable]
public struct AreaUIBlock
{
    public GameObject obj;
    public Image image;
}