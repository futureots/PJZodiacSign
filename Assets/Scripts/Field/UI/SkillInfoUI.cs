using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour
{
    public Image skillIcon;
    public TextMeshProUGUI skillTitle;
    public TextMeshProUGUI skillDescription;

    public void SetSkillUI(BaseSkillData skillData)
    {
        skillIcon.sprite = skillData.skillIcon;
        skillTitle.text = skillData.skillName;
        skillDescription.text = skillData.skillDescription;
    }
}
