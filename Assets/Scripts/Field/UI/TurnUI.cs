using PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : InputManagerUI
{
    [SerializeField] InputManager inputManager;
    [SerializeField] Button turnEndBtn;
    [SerializeField] TextMeshProUGUI text;

    public  override void Init(InputManager input)
    {
        this.inputManager = input;
        input.OnModeChanged += OnModeChange;
        input.agent.fieldController.OnTurnStarted += OnTurnChange;
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

    void OnTurnChange(Turn turn)
    {
        if(turn.agentID == inputManager.agent.id)
        {
            switch (turn.type)
            {
                case TurnType.REPAIR:
                    text.text = "정비 종료";
                    break;
                case TurnType.ACTION:
                    text.text = "행동 종료";
                    break;
                case TurnType.ATTACK:
                    text.text = "공격 중";
                    break;
                default:
                    break;
            }
        }
        else
        {
            text.text = "상대 턴";
        }

    }
}
