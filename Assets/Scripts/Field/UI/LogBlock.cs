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
        var pos = deadEntity.curTile.fieldPos;
        var text = $"( {(char)((pos.x - 1)+'A')}, {pos.y} )";


        commandText.text = $"{text} {deadEntity.baseData.productName} 사망";
    }
}
