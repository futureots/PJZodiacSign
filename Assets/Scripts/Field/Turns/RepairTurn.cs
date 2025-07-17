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
        foreach (var controller in GameManager.Instance.controllers)
        {
            // 필드 앞에 인스턴트 필드 생성 및 보유 중인 기물 표시하기
            controller.SetInstantField();
            // 메인 필드에 기물 배치하기
            controller.SetMainField();
        }

        int completeUsers = 0;
        foreach (var agent in GameManager.Instance.teams)
        {
            agent.SetMode(Mode.Repair, () =>
            {
                agent.SetMode(Mode.None);
                completeUsers++;
                // 모든 유저가 준비 완료 시
                if (completeUsers >= GameManager.Instance.teams.Length)
                {
                    // 각 컨트롤러 별 제거 및 플레이어 수에 따라 턴 넘기기로 변경 필요
                    foreach (var controller in GameManager.Instance.controllers)
                    {
                        controller.DisposeInstantField();
                    }
                    onTurnEnd?.Invoke();
                }
            });

        }
    }
}
