using PlayerInput;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    public Button turnEndBtn;


    public void Init(InputManager input)
    {
        this.inputManager = input;
        input.OnModeChanged += OnModeChange;
        turnEndBtn.onClick.AddListener(TurnEnd);
    }

    void OnModeChange(IInputState state)
    {
        if(state is MoveModeInput)
        {
            turnEndBtn.interactable = true;
        }
        else
        {
            turnEndBtn.interactable = false;
        }
    }
    void TurnEnd()
    {
        inputManager.ClearInputMode();
        turnEndBtn.interactable = false;

        inputManager.agent.CreateEndCommand();
    }
}
