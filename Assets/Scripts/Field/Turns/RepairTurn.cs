using PlayerInput;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RepairTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {

        // 상점 UI 표시하기
        //정비 시작
        Debug.Log("정비 시작");



        GameManager.Instance.field.EraseField();
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetData();
        }

        int completeUsers = 0;
        foreach (var agent in GameManager.Instance.agents)
        {
            agent.SetMode(Mode.Repair, () =>
            {
                agent.SetMode(Mode.None);
                completeUsers++;
                // 모든 유저가 준비 완료 시
                if (completeUsers >= GameManager.Instance.agents.Length)
                {
                    agent.UpdateEntities();
                    // 각 컨트롤러 별 제거 및 플레이어 수에 따라 턴 넘기기로 변경 필요
                    /*foreach (var controller in GameManager.Instance.controllers)
                    {
                        controller.DisposeInstantField();
                    }*/
                    onTurnEnd?.Invoke();
                }
            });

        }
    }
}
