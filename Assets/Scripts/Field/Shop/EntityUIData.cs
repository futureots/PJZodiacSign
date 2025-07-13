using UnityEngine;

[CreateAssetMenu(fileName = "EntityUIData", menuName = "Scriptable Objects/EntityUIData")]
public class EntityUIData : ScriptableObject
{
    public string id;
    public string productName;
    public Sprite icon;
    public int normalPrice;
    public int discountPrice;


}
