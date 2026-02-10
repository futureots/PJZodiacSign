using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostUI : MonoBehaviour
{
    InputManager _inputManager;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    
    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.agent.fieldController.OnTurnStarted += OnTurnChange;
    }

    void OnTurnChange(Turn turn)
    {
        if (turn.agentID == _inputManager.agent.id && turn.type == TurnType.ACTION)
        {
            panel.SetActive(true);
            _inputManager.agent.onActionCountChanged += OnActionCountChange;
            OnActionCountChange(_inputManager.agent.CurrentActionCount);
        }
        else
        {
            panel.SetActive(false);
            _inputManager.agent.onActionCountChanged -= OnActionCountChange;
        }
    }

    void OnActionCountChange(int actionCount)
    {
        text.text = $"{actionCount}/{_inputManager.agent.actionCount}";
    }
}
