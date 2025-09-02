using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn : ITurn
{
    public static Action<Agent, Command> OnCommandExecuted;
    Agent agent;
    public Agent Agent
    {
        get { return agent; }
    }
    public ActionTurn(Agent agent)
    {
        this.agent = agent;
    }
    public void StartTurn(Action onTurnEnd)
    {
        agent.SetActionTurn(() => GameManager.Instance.RunWithCallback(ActionCoroutine(), onTurnEnd));

        foreach (var entity in agent.controller.entities)
        {
            entity.CurEnergy = Mathf.Min(entity.CurEnergy + 1, entity.SkillCost);
        }
    }
    public IEnumerator ActionCoroutine()
    {
        var cmd = agent.GetCommand();
        //Debug.Log(cmd.ToString());
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
