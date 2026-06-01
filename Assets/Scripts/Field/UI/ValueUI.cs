using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image entityImage;

    public void Init(Sprite entityIcon, int count)
    {
        valueText.text = $"+{count}";
        entityImage.sprite = entityIcon;
    }
}
