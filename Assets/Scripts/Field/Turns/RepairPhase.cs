using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RepairPhase : IPhase
{
    int level;
    public int Level
    {
        get
        {
            return level;
        }
    }

    public RepairPhase(int level)
    {
        this.level = level;
    }
    public void StartPhase(Action onPhaseEnd)
    {
        Debug.Log($"level {level} : 수리 페이즈 시작");
        PhaseManager.curPhase = PhaseType.Repair;

        
        GameManager.Instance.field.ResetField();
        int completeUsers = 0;
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetRepairPhase(level, () =>
            {
                agent.EndRepairPhase();
                completeUsers++;
                // 모든 플레이어가 수리 완료 시
                if (completeUsers >= GameManager.Instance.agents.Length)
                {
                    onPhaseEnd?.Invoke();
                }
            });
        }
    }
}

