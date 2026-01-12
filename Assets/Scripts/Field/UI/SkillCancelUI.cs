using PlayerInput;
using UnityEngine;
using UnityEngine.UI;

public class SkillCancelUI : MonoBehaviour
{
    public Button cancelBtn;
    public void Init(InputManager inputManager)
    {
        inputManager.onModeChanged +=OnModeChange;
    }

    void OnModeChange(IInputState state)
    {
        if(state is SkillModeInput skillState)
        {
            cancelBtn.gameObject.SetActive(true);
            cancelBtn.onClick.AddListener(() => skillState.onCanceled?.Invoke());
        }
        else
        {
            cancelBtn.gameObject.SetActive(false);
            cancelBtn.onClick.RemoveAllListeners();
        }
    }
}
