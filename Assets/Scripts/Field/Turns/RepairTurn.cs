using System;
using UnityEngine;

public class RepairTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        // 필드 앞에 인스턴트 필드 생성 및 보유 중인 기물 표시하기
        // 상점 UI 표시하기
        // 플레이어 입력 모드 변경 및 종료 버튼에 콜백 추가
        //정비 시작
        Debug.Log("정비 시작");
        InputManager.Instance.SetInputMode(InputManager.Mode.Repair);
        InputManager.Instance.turnEndButton.onClick.AddListener(() =>
        {
            // 종료 버튼 선택 시 기물 위치 확정 카메라 위치 조정, 인스턴트 필드 제거, UI 버튼 비활성화
            InputManager.Instance.SetInputMode(InputManager.Mode.None);
            onTurnEnd?.Invoke();
        });
    }
}
