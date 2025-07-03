using System;
using UnityEngine;

public class RepairTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        var controller = InputManager.Instance.controller;
        
        // 상점 UI 표시하기
        //정비 시작
        Debug.Log("정비 시작");
        // 필드 앞에 인스턴트 필드 생성 및 보유 중인 기물 표시하기
        InputManager.Instance.SetInputMode(InputManager.Mode.Repair);
        controller.instantField.gameObject.SetActive(true);
        foreach (var item in DataManager.Instance.playerData.handEntities)
        {
            var resource = ResourceManager.GetEntityResource(item.name);
            var instance = GameObject.Instantiate(resource);
            var entity = instance.GetComponent<Entity>();
            controller.PushEntity(entity, controller.instantField);
        }
        // 필드 정리 및 기존에 존재하던 기물 설치
        GameManager.Instance.field.EraseField();
        foreach (var item in DataManager.Instance.playerData.fieldEntities)
        {
            var resource = ResourceManager.GetEntityResource(item.Value.name);
            var instance = GameObject.Instantiate(resource);
            var entity = instance.GetComponent<Entity>();
            // key를 좌표 값으로 전환

            intVector2 vec = intVector2.Decode(item.Key);
            controller.SetEntity(entity, GameManager.Instance.field, vec);
        }

        InputManager.Instance.turnEndButton.onClick.AddListener(() =>
        {
            // 종료 버튼 선택 시 기물 위치 확정 카메라 위치 조정, 인스턴트 필드 제거, UI 버튼 비활성화
            InputManager.Instance.SetInputMode(InputManager.Mode.None);
            // 플레이어 필드 데이터 재구성(추가로 배치한 기물의 정보를 데이터에 추가, 이후 해당 전투 종료 시 데이터 저장)


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
