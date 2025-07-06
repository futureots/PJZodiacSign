using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepairTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        
        // 상점 UI 표시하기
        //정비 시작
        Debug.Log("정비 시작");

        InputManager.Instance.SetInputMode(InputManager.Mode.Repair);
        GameManager.Instance.field.EraseField();
        foreach (var controller in GameManager.Instance.controllers)
        {
            // 필드 앞에 인스턴트 필드 생성 및 보유 중인 기물 표시하기
            controller.SetInstantField();
            // 메인 필드에 기물 배치하기
            controller.SetMainField();
        }

        // 종료 버튼 선택 시 기물 위치 확정 카메라 위치 조정, 인스턴트 필드 제거, UI 버튼 비활성화
        InputManager.Instance.turnEndButton.onClick.AddListener(() =>
        {
            
            InputManager.Instance.SetInputMode(InputManager.Mode.None);

            // 적 필드의 기물 설치(멀티는 양쪽의 턴종료 버튼 입력 시 다음 턴으로 이동)
            foreach (var controller in GameManager.Instance.controllers)
            {
                controller.DisposeInstantField();
            }
            InputManager.Instance.turnEndButton.onClick.RemoveAllListeners();
            onTurnEnd?.Invoke();
        });
    }
}
