using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ActionTurn : ITurn
{
    public static Action<Agent, Command> OnCommandExecuted;
    Agent agent;
    public int TeamNumber => agent.team.teamNumber;
    public ActionTurn(Agent agent)
    {
        this.agent = agent;
    }
    public void StartTurn(Action onTurnEnd)
    {
        agent.SetActionTurn(() => GameManager.Instance.RunWithCallback(ActionCoroutine(), onTurnEnd));

        // 버프 업데이트
        foreach (var entity in agent.controller.entities)
        {
            entity.OnTurnStart();
        }
    }
    public IEnumerator ActionCoroutine()
    {
        var cmd = agent.GetCommand();
        OnCommandExecuted?.Invoke(agent, cmd);
        if (cmd == null) Debug.Log("No Command");
        cmd?.Execute();


        yield return new WaitForSeconds(2f);
    }

    public string GetTurnInfo()
    {
        return $"팀 {agent.team.teamNumber} 행동 ";
    }
}
