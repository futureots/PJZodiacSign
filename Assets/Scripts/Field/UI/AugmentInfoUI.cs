using Augment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentInfoUI : MonoBehaviour
{
    public Image frame;
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Button selectButton;

    public void Init(AugmentSO augmentSO)
    {
        icon.sprite = augmentSO.icon;
        nameText.text = augmentSO.EffectName;
        descriptionText.text = augmentSO.Description;

        frame.color = AugmentSO.Color[augmentSO.Type];
    }
}
