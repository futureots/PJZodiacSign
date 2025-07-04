using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

            // 추가로 배치한 기물을 데이터에 업데이트
            var tiles = GameManager.Instance.field.GetHalfTiles(InputManager.Instance.controller.isReflect);
            Dictionary<int,EntityData> fieldDatas = new Dictionary<int,EntityData>();
            foreach (var tile in tiles)
            {
                if (tile.isEmpty) continue;
                var entity = tile.occupiedObject.GetComponent<Entity>();
                var entityData = new EntityData(entity);
                int pos = tile.fieldPos.Encode();
                fieldDatas.Add(pos, entityData);
                Debug.Log($"data {entityData.name} : pos {pos}");
            }
            DataManager.Instance.playerData.fieldEntities = fieldDatas;

            // 인스턴트 필드에 남은 기물을 데이터에 업데이트
            List<EntityData> handDatas = new List<EntityData>();
            foreach (var tile in InputManager.Instance.controller.instantField.GetTiles())
            {
                if(tile.isEmpty) continue;
                var entity = tile.occupiedObject.GetComponent<Entity>();
                var entityData = new EntityData(entity);
                handDatas.Add(entityData);
            }
            DataManager.Instance.playerData.handEntities = handDatas;


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
