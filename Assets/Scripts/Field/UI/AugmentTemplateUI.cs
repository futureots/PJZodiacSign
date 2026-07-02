using Augment;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class AugmentTemplateUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AugmentSO _augmentData;
    
    public event Action<Vector2,AugmentSO> OnMouseHover;

    [SerializeField] private Image iconContent;
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;
    [SerializeField] private Color neutralColor;

    public void Init(AugmentSO data)
    {
        _augmentData = data;
        iconContent.sprite = data.icon;
        iconContent.color = data.Type switch
        {
            AugmentType.Positive => positiveColor,
            AugmentType.Negative => negativeColor,
            _ => neutralColor
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EditorLogger.Print("OnPointerEnter");
        OnMouseHover?.Invoke(transform.position,_augmentData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EditorLogger.Print("OnPointerExit");
        OnMouseHover?.Invoke(transform.position,null);
    }
}
