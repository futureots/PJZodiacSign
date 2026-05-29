using TMPro;
using UnityEngine;

public class GoodsInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goodsDescription;

    public void SetInfo(AbstractData data)
    {
        if (data is EntityData entityData)
        {
            
        }
        else if (data is ItemData itemData)
        {
            
        }
    }
}
