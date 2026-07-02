using Augment;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentUI : MonoBehaviour
{
    private AugmentManager manager;
    public Transform iconContent;
    private readonly List<AugmentTemplateUI> currentAugments = new();
    
    [Header("Icon Template")]
    [SerializeField] private AugmentTemplateUI augTemplate;
    [SerializeField] private RectTransform descriptionPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    

    private void Awake()
    {
        manager = FindAnyObjectByType(typeof(AugmentManager)) as AugmentManager;
        if (manager != null)
        {
            manager.OnAugmentChanged += Refresh;
        }
    }

    private void Refresh(List<AugmentSO> augments)
    {
        // 리스트 새로고침
        foreach (var argIcon in currentAugments)
        {
            Destroy(argIcon.gameObject);
        }
        currentAugments.Clear();
        
        // 새로 생성
        foreach (var augment in augments)
        {
            var icon = Instantiate(augTemplate, iconContent);
            icon.Init(augment);
            icon.OnMouseHover += OnMouseHover;
            currentAugments.Add(icon);
        }
    }

    void OnMouseHover(Vector2 pos, AugmentSO augment)
    {
        if (augment == null)
        {
            descriptionPanel.gameObject.SetActive(false);
            return;
        }
        descriptionPanel.gameObject.SetActive(true);
        descriptionPanel.position = pos;
        descriptionText.text = augment.Description;
        titleText.text = augment.EffectName;
    }
}
