using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RepairPhase : IPhase
{
    IPhaseManageService _service;
    Field field;
    int level;
    public int Level
    {
        get
        {
            return level;
        }
    }
    public PhaseType PhaseType => PhaseType.Repair;

    public RepairPhase(int level,Field mainField)
    {
        this.level = level;
        field = mainField;
    }
    public void StartPhase(Action onPhaseEnd)
    {
        Debug.Log($"level {level} : 수리 페이즈 시작");

        
        field.ResetField();
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

