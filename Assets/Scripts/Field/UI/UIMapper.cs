using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum UIType
{
    None,
    Option = 1 << 0,
    PlayerData =  1 << 1,
    EntityInfo = 1 << 2,
    Inventory = 1 << 3,
    Turn = 1 << 4,
    Common = Option | PlayerData | EntityInfo | Inventory | Turn,
    Shop =  1 << 5,
    Resource = 1 << 6,
    Log = 1 << 7,
}

[Serializable]
public struct UIMap
{
    public UIType type;
    public GameObject uiPanel;      // NOTE: UI Panel 추상 타입으로 개선
}

public class UIMapper : Singleton<UIMapper>
{
    /**
     * Model UI 관리자
     * - 타입에 따른 UI 객체 맵핑
     * - UI 사용 플래그를 이용하여 UI 활성화 관리
     */
    [SerializeField] private List<UIMap> uiMaps;
    private readonly Dictionary<UIType, GameObject> _container = new();

    private void Start()
    {
        RefreshDictionary();
        
    }

    /// <summary>
    /// Refresh UI Container Dictionary
    /// </summary>
    private void RefreshDictionary()
    {
        // Reset Container
        _container.Clear();

        // Bake Dictionary
        foreach (var ui in uiMaps)
        {
            if (ui.uiPanel && !_container.ContainsKey(ui.type))
            {
                _container.Add(ui.type, ui.uiPanel);
                
                ui.uiPanel.SetActive(false);        // Default Inactive
            }
        }
    }

    /// <summary>
    /// Active UI for Phase
    /// </summary>
    /// <param name="uiFlag">Flag for UI to use</param>
    public void SetUI(UIType uiFlag)
    {
        foreach (var map in uiMaps)
        {
            if (!map.uiPanel) continue;     // use Only when UI Exists

            bool active = uiFlag.HasFlag(map.type);
            map.uiPanel.SetActive(active);
        }
    }
}

public abstract class UI : MonoBehaviour
{
    public virtual void Init(InputManager input) { }
}
