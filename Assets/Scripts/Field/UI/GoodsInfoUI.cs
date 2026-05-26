using TMPro;
using UnityEngine;

public class GoodsInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goodsInfoUI;

    public void SetText(string text)
    {
        goodsInfoUI.text = text;
    }
}
