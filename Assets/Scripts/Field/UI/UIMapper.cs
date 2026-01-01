using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum UIType
{
    None,
    PlayerData = 1 << 0,
    EntityInfo =  1 << 1,
    Turn = 1 << 2,
    Option = 1 << 3,
    Shop =  1 << 4,
    Resource = 1 << 5,
    Log = 1 << 6,
    Inventory =  1 << 7,
}

[Serializable]
public struct UIMap
{
    public UIType type;
    public GameObject uiPanel;      // NOTE: UI Panel 추상 타입으로 개선
}

public class UIMapper
{
    [SerializeField] private List<UIMap> uiMaps;
    private Dictionary<UIType, GameObject> container = new();

    /// <summary>
    /// Refresh UI Container Dictionary
    /// </summary>
    private void RefreshDictionary()
    {
        // Reset Container
        container.Clear();

        // Bake Dictionary
        foreach (var ui in uiMaps)
        {
            if (ui.uiPanel && !container.ContainsKey(ui.type))
            {
                container.Add(ui.type, ui.uiPanel);
                
                ui.uiPanel.SetActive(false);        // Default Inactive
            }
        }
    }
    
    
}