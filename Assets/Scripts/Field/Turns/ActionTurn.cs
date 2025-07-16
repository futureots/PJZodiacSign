using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn : ITurn
{
    Agent agent;
    public ActionTurn(Agent agent)
    {
        this.agent = agent;
    }
    public void Execute(Action onTurnEnd)
    {
        Debug.Log(agent.tag + "행동 턴 시작");
        // 현재 플레이어의 행동 턴 일 경우
        agent.SetMode(Mode.Move, () => GameManager.Instance.RunWithCallback(ActionCoroutine(), onTurnEnd));

    }
    public IEnumerator ActionCoroutine()
    {
        Debug.Log(agent.tag +" 행동 턴 실행");
        agent.isInputStop = true;
        agent.SetMode(Mode.None);
        

        var cmd = agent.GetCommand();
        if (cmd == null) Debug.Log("No Command");
        cmd?.Execute();


        yield return new WaitForSeconds(0.1f);
        agent.isInputStop = false;
    }
}
