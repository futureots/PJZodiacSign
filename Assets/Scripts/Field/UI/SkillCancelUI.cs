using PlayerInput;
using UnityEngine;
using UnityEngine.UI;

public class SkillCancelUI : InputManagerUI
{
    public Button cancelBtn;
    public  override void Init(InputManager inputManager)
    {
        inputManager.onModeChanged +=OnModeChange;
    }

    void OnModeChange(IInputState state)
    {
        if(state is SkillModeInput skillState)
        {
            // 입력을 진행하는 동안에만 스킬 버튼 활성화 및 기능 추가
            skillState.onInputStarted += cancelBtn.gameObject.SetActive;
            cancelBtn.onClick.AddListener(() => skillState.onCanceled?.Invoke());
        }
        else
        {
            cancelBtn.gameObject.SetActive(false);
            cancelBtn.onClick.RemoveAllListeners();
        }
    }
}
