using System;
using System.Collections;
using UnityEditor.UIElements;

using UnityEngine;

public class AttackTurn : ITurn
{
    Agent agent;

    public Agent Agent
    {
        get { return agent; }
    }

    public AttackTurn(Agent agent)
    {
        this.agent = agent;
    }

    public void StartTurn(Action onTurnEnd)
    {
        Debug.Log("공격 턴 시작");

        GameManager.Instance.RunWithCallback(AttackCoroutine(), onTurnEnd);
    }
    public IEnumerator AttackCoroutine()
    {
        // 현재 전투 중인 필드;
        Field curField = GameManager.Instance.field;
        // 해당 팀 반대 기물만 공격
        foreach (var obj in curField.GetOccupiedObjects())
        {
            if (agent.team.isAlly(obj.GetComponent<Team>()))
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

        curField.RemoveDeadEntities();

        // 모든 캐릭터 버프 업데이트
        foreach (var tile in curField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            if (entity == null) continue;
            entity.UpdateBuff();
            entity.RemoveBuff();
            
        }


    }

    public string GetTurnInfo()
    {
        return $"팀 {agent.team.teamNumber} 공격 ";
    }
}


