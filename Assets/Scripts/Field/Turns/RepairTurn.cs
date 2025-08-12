using PlayerInput;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RepairTurn : ITurn
{
    int level;
    public RepairTurn(int level)
    {
        this.level = level;
    }
    public void StartTurn(Action onTurnEnd)
    {
        
        // 수리 UI 표시하기
        //수리 시작
        Debug.Log($"level {level} : 수리 시작");



        GameManager.Instance.field.EraseField();
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetRepairField(level);
        }

        int completeUsers = 0;
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetMode(Mode.Repair, () =>
            {
                agent.SetMode(Mode.None);
                completeUsers++;
                // 모든 플레이어가 수리 완료 시
                if (completeUsers >= GameManager.Instance.agents.Length)
                {
                    foreach (Agent agent in GameManager.Instance.agents)
                    {
                        agent.EndRepair();
                    }
                    onTurnEnd?.Invoke();

                    // 체력바 UI 표시
                    GameManager.Instance.SetEntityHpBar();
                }
            });

        }
    }
}
