using PlayerInput;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    InputManager _inputManager;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField, TextArea(3,5)] string repairDescription;
    [SerializeField, TextArea(3, 5)] string moveDescription;
    [SerializeField, TextArea(3, 5)] string skillDescription;
    [SerializeField, TextArea(3, 5)] string defaultDescription;

    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.OnModeChanged += OnModeChange;
    }

    void OnModeChange(IInputState state)
    {
        string text = state switch
        {
            RepairModeInput => repairDescription,
            MoveModeInput => moveDescription,
            SkillModeInput => skillDescription,
            _ => defaultDescription
        };
        description.text = text;
    }
}
