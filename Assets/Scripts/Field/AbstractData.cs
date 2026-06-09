using UnityEngine;
using UnityEngine.Localization;


public class AbstractData : ScriptableObject
{
    public string id;
    public string ProductName => productName.GetLocalizedString();
    [SerializeField] protected LocalizedString productName; 
    public Sprite icon;
    public int normalPrice;
}
