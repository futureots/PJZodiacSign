using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogBlock : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI commandText;
    public Image backgroundImage;


    public void Initialize(Entity deadEntity)
    {
        commandText.text = $"({deadEntity.curTile.fieldPos}){deadEntity.baseData.productName} 사망";
    }
}
