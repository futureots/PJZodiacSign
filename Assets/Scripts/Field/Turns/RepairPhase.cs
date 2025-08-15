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
        PhaseManager.curPhase = PhaseType.Repair;

        // 입력 후 커맨드 생성 시 즉시 실행되는 액션 추가(현재 정비 턴은 입력 종료시 커맨드 안만들고 즉시 실행하는데 커맨드 생성방식으로 바꾸고 커맨드 생성 시 즉시 실행되도록 만들기)
        

        GameManager.Instance.field.ResetField();
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetRepairPhase(level);
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
                        agent.EndRepairPhase();
                    }
                    onPhaseEnd?.Invoke();

                    // 체력바 UI 표시
                    GameManager.Instance.SetEntityHpBar();
                }
            });
        }
    }
}

