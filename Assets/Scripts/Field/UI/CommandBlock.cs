using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandBlock : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI description;
    public Image backgroundImage;

    public int index { get; private set; }
    public void Init(int index)
    {
        this.index = index;
    }
    public void SetCommand(Command command)
    {
        var text = command.ToString();
        description.text = text;
    }
}
