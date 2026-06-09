using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TurnUI : InputManagerUI
{
    [SerializeField] InputManager inputManager;
    [SerializeField] Button turnEndBtn;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] private LocalizedString repairTurn;
    [SerializeField] private LocalizedString actionTurn;
    [SerializeField] private LocalizedString attackTurn;
    [SerializeField] private LocalizedString opponentTurn;

    public  override void Init(InputManager input)
    {
        this.inputManager = input;
        input.onModeChanged += OnModeChange;
        input.agent.fieldController.OnTurnStarted += OnTurnChange;
        turnEndBtn.onClick.AddListener(TurnEnd);
    }

    void OnModeChange(IInputState state)
    {
        if(state is MoveModeInput)
        {
            if (state is RepairModeInput repair)
            {
                repair.onEntityCountChanged += OnEntityCountChanged;
            }
            else
            {
                turnEndBtn.interactable = true;
            }
            
        }
        else
        {
            turnEndBtn.interactable = false;
        }
    }

    private void OnEntityCountChanged(int cur, int max)
    {
        if (cur == 0)
        {
            turnEndBtn.interactable = false;
        }
        else
        {
            turnEndBtn.interactable = true;
        }
    }

    void TurnEnd()
    {
        inputManager.ClearInputMode();
        turnEndBtn.interactable = false;

        inputManager.agent.CreateEndCommand();
    }

    void OnTurnChange(Turn turn, uint count)
    {
        if(turn.agentID == inputManager.agent.id)
        {
            switch (turn.type)
            {
                case TurnType.REPAIR:
                    text.text = repairTurn.GetLocalizedString();
                    break;
                case TurnType.ACTION:
                    text.text = actionTurn.GetLocalizedString();
                    break;
                case TurnType.ATTACK:
                    text.text = attackTurn.GetLocalizedString();
                    break;
                default:
                    break;
            }
        }
        else
        {
            text.text = opponentTurn.GetLocalizedString();
        }

    }
}
