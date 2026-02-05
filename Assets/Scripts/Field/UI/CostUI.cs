using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostUI : MonoBehaviour
{
    InputManager _inputManager;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] CommandBlock prefab;
    [SerializeField] Transform container;

    List<CommandBlock> blocks= new();
    
    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        _inputManager.agent.fieldController.OnTurnStarted += OnTurnChange;
        blocks.Clear();
        for (int i = 0; i < _inputManager.agent.actionCount; i++)
        {
            var obj = Instantiate(prefab,container);
            obj.Init(i);
            blocks.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }

    void OnTurnChange(Turn turn)
    {
        if (turn.agentID == _inputManager.agent.id && turn.type == TurnType.ACTION)
        {
            panel.SetActive(true);
            _inputManager.agent.fieldController.OnListUpdated += OnListUpdate;
        }
        else
        {
            panel.SetActive(false);
            _inputManager.agent.fieldController.OnListUpdated -= OnListUpdate;
        }
    } 

    void OnListUpdate(List<Command> curCmds)
    {
        int count = _inputManager.agent.actionCount;
        text.text =  Mathf.Max(count - curCmds.Count,0) + " / " + count;

        for (int i = 0; i < count; i++)
        {
            if (curCmds.Count > i)
            {
                blocks[i].gameObject.SetActive(true);
                blocks[i].SetCommand(curCmds[i]);
            }
            else
            {
                blocks[i].gameObject.SetActive(false);
            }
        }
    }
}
