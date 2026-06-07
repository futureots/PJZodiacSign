using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image skillIcon;
    public TextMeshProUGUI skillTitle;
    public TextMeshProUGUI skillDescription;
    
    public AreaUIBlock skillArea;
    private bool _hasArea;
    
    /// <summary>
    /// 스킬 UI 설정
    /// </summary>
    /// <param name="skillData">세팅할 스킬 데이터</param>
    public void SetSkillUI(BaseSkillData skillData)
    {
        if (skillData == null)
        {
            skillTitle.text = "스킬 없음";
            skillDescription.text = string.Empty;
        }
        else
        {
            skillIcon.sprite = skillData.skillIcon;
            skillTitle.text = skillData.skillName;
            skillDescription.text = skillData.skillDescription;
        }

        if (skillData.skillLogic is AreaSkillLogic areaSkillLogic)
        {
            _hasArea = true;
            skillArea.image.sprite = areaSkillLogic.Area.areaImage;
        }
        else
        {
            _hasArea = false;
        }
        skillArea.obj.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_hasArea)
        {
            skillArea.obj.SetActive(true);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillArea.obj.SetActive(false);
    }
}
