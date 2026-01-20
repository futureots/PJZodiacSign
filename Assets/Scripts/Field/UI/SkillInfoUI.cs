using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillTitle;
    public TextMeshProUGUI skillDescription;

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
    }
}
