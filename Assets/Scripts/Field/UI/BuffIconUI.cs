using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI count;
    public void SetBuffIcon(BuffInstance buff)
    {
        icon.sprite = buff.buffData.buffIcon;
        count.text = buff.count.ToString();
    }
}
