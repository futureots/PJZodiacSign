using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ActionTurn : ITurn
{
    public Action<Command> onCommandExecuted;
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
        foreach (var entity in agent.controller.fieldEntities)
        {
            if(entity.TryGetComponent<SkillComponent>(out var skill))
            {
                skill.RegenerateEnergy();
            }

            if(entity.TryGetComponent<BuffManager>(out var buffs))
            {
                buffs.UpdateBuff();
                buffs.RemoveBuff();
            }
        }
    }
    public IEnumerator ActionCoroutine()
    {
        var cmd = agent.GetCommand();
        if (cmd == null) Debug.Log("No Command");
        cmd?.Execute();
        onCommandExecuted?.Invoke(cmd);


        yield return new WaitForSeconds(2f);
    }

}
