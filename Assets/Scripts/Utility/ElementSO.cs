using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Element")]
public class ElementSO : ScriptableObject
{
    public List<Material> elementMaterials;

    public Material GetMaterial(Element element)
    {
        var num = (int)element;
        if (num < elementMaterials.Count)
        {
            return elementMaterials[num];
        }
        return null;
    }
}
public enum Element
{
    //상성 없음(항상 최상위? 최하위?)
    Empty = 0,
    Tree = 1,
    Fire = 2,
    Dirt = 3,
    Metal = 4,
    Water = 5,
}