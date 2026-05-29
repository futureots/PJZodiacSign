using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goodsDescription;
    [SerializeField] private AreaUIBlock moveArea;
    [SerializeField] private AreaUIBlock attackArea;
    [SerializeField] private AreaUIBlock skillArea;
    public void SetInfo(AbstractData data)
    {
        if (data is EntityData entityData)
        {
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

            goodsDescription.text = entityData.skill.skillDescription;
        }
        else if (data is ItemData itemData)
        {
            
        }
    }
}

[System.Serializable]
public struct AreaUIBlock
{
    public GameObject obj;
    public Image image;
}