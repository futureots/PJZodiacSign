using Augment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour
{
    private AugmentManager manager;
    public Transform iconContent;
    private readonly List<(Image icon, AugmentSO data)> currentAugments = new();
    
    [Header("Icon Template")]
    [SerializeField] private GameObject positiveTemplate;
    [SerializeField] private GameObject negativeTemplate;
    [SerializeField] private GameObject neutralTemplate;

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
            Destroy(argIcon.icon.gameObject);
        }
        currentAugments.Clear();
        
        // 새로 생성
        foreach (var augment in augments)
        {
            var template = augment.Type switch
            {
                AugmentType.Positive => positiveTemplate,
                AugmentType.Negative => negativeTemplate,
                _ => neutralTemplate
            };
            
            var newIcon = Instantiate(template, iconContent).GetComponent<Image>();
            newIcon.gameObject.SetActive(true);
            
            currentAugments.Add((newIcon, augment));
        }
    }
}
