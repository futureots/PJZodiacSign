using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RepairPhase : IPhase
{
    int level;
    
    public RepairPhase(int level)
    {
        this.level = level;
    }
    
    public void StartPhase(Action onPhaseEnd)
    {
        Debug.Log($"level {level} : 수리 페이즈 시작");

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
                    onPhaseEnd?.Invoke();

                    // 체력바 UI 표시
                    GameManager.Instance.SetEntityHpBar();
                }
            });
        }
    }
}

