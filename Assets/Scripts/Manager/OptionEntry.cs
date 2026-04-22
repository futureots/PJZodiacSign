using GlobalManage;
using UnityEngine;
using UnityEngine.UI;

public class OptionEntry : MonoBehaviour
{
    private Button button;

    public void OnButtonClick()
    {
        if (GameOption.Instance)
        {
            GameOption.Instance.ActiveOption(true);
        }
    }
}
