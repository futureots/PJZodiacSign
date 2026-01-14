using PlayerInput;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    InputManager _inputManager;
    public Button turnEndBtn;

    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        inputManager.onModeChanged += OnModeChange;
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
        _inputManager.agent.CreateEndCommand();
        _inputManager.agent.SendCommand();
        _inputManager.ClearInputMode();
        turnEndBtn.interactable = false;
    }
}
