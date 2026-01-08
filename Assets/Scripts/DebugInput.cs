using PlayerInput;
using UnityEngine;
using UnityEngine.UI;

public class DebugInput : MonoBehaviour
{
    public InputManager inputManager;

    public Button turnEndButton;
    public Button skillButton;
    public Button skillCancelButton;

    public SkillComponent skillComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillComp = GetComponent<SkillComponent>();
        inputManager = FindFirstObjectByType<InputManager>();
        inputManager.onModeChanged += (state) =>
        {
            EditorLogger.Print("ModeChange");
            if (state is MoveModeInput)
            {
                turnEndButton.interactable = true;
                skillButton.interactable = true;
                skillCancelButton.gameObject.SetActive(false);
            }
            else if(state is SkillModeInput)
            {
                
                turnEndButton.interactable = false;
                skillButton.interactable = false;
                skillCancelButton.gameObject.SetActive(true);
            }
            else
            {
                turnEndButton.interactable = false;
                skillButton.interactable = false;
                skillCancelButton.gameObject.SetActive(false);
            }
        };
        skillCancelButton.onClick.AddListener(CancelSkill);
        skillButton.onClick.AddListener(UseSkill);
        turnEndButton.onClick.AddListener(TurnEnd);
    }

    
    public void TurnEnd()
    {
        inputManager.agent.CreateEndCommand();
        inputManager.agent.SendCommand();
        inputManager.ClearInputMode();
    }

    public void UseSkill()
    {
        inputManager.SetInputMode(skillComp);
    }

    public void CancelSkill()
    {
        inputManager.onCanceled?.Invoke();
    }

}
