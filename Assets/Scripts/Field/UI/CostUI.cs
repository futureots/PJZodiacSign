using PlayerInput;
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
        _inputManager.agent.fieldController.OnListUpdated += OnListUpdate;
    }

    void OnTurnChange(Turn turn)
    {
        panel.SetActive(false);
        if (turn.agentID == _inputManager.agent.id)
        {
            if(turn.type == TurnType.ACTION)
            {
                panel.SetActive(true);
            }
        }

    } 

    void OnListUpdate(int curCmdCount)
    {
        int count = _inputManager.agent.actionCount;
        text.text =  Mathf.Max(count - curCmdCount,0) + " / " + count;
    }
}
