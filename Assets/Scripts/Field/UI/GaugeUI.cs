using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    public Image gaugeBar;
    public TextMeshProUGUI gaugeText;

    [ContextMenu("SetComponent")]
    public void SetComponent()
    {
        gaugeBar = GetComponent<Image>();
        gaugeText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetGauge(int value, int max)
    {
        gaugeBar.fillAmount = Mathf.Min((float)value / max,1);
        gaugeText.text = $"{value} / {max}";
    }
}
