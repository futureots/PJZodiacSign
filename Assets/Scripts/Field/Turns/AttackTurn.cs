using System;
using System.Collections;
using UnityEditor.UIElements;

using UnityEngine;

public class AttackTurn : ITurn
{
    Agent agent;

    public int TeamNumber => agent.team.teamNumber;

    public AttackTurn(Agent agent)
    {
        this.agent = agent;
    }

    public void StartTurn(Action onTurnEnd)
    {

        GameManager.Instance.RunWithCallback(AttackCoroutine(), onTurnEnd);
    }
    public IEnumerator AttackCoroutine()
    {
        // 현재 전투 중인 필드;
        Field curField = GameManager.Instance.field;
        foreach(var entity in agent.controller.fieldEntities)
        {
            entity.Attack();
        }

        // 중립 오브젝트 실행
        foreach (var obj in curField.GetOccupiedObjects())
        {
            if (obj.GetComponent<Team>())
            {
                continue;
            }
            var attackable = obj.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attack();
            }
        }

        // 대기시간
        yield return new WaitForSeconds(2f);


    }

    public string GetTurnInfo()
    {
        return $"팀 {agent.team.teamNumber} 공격 ";
    }
}


