
using System;
using UnityEngine;

[Serializable]
public enum UIType
{
    Inventory,
    PlayerData,
    EntityInfo,
    Turn,
    Option,
    Shop,
    Resource,
}

[Serializable]
public struct UIMap
{
    public UIType type;
    public GameObject uiPanel;
}

public class UIMapper
{
    
}