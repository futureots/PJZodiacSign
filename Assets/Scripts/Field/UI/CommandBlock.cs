using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandBlock : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI commandText;
    public Image backgroundImage;


    public void Init(Command command, Agent agent)
    {
        Color teamColor = GameManager.Instance.teamColorTable.teamColors[agent.team.teamNumber];

        backgroundImage.color = teamColor;

        commandText.text = command.ToString();
    }
}
