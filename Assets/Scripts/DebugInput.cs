using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugInput : MonoBehaviour
{
    public InputManager inputManager;

    public Button turnEndButton;
    public Button skillButton;
    public Button skillCancelButton;
    public TextMeshProUGUI text;

    public SkillComponent skillComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillComp = GetComponent<SkillComponent>();
        inputManager.onModeChanged += (state) =>
        {
            if (state is MoveModeInput)
            {
                turnEndButton.interactable = true;
                skillButton.interactable = true;
                skillCancelButton.gameObject.SetActive(false);
                text.text = "MoveMode";
            }
            else if(state is SkillModeInput)
            {
                turnEndButton.interactable = false;
                skillButton.interactable = false;
                skillCancelButton.gameObject.SetActive(true);
                text.text = "SkillMode";
            }
            else
            {
                turnEndButton.interactable = false;
                skillButton.interactable = false;
                skillCancelButton.gameObject.SetActive(false);
                text.text = "OtherElse";
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
        //inputManager.onCanceled?.Invoke();
    }

}
