using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : EntityController
{
    public override void SetInstantField()
    {
        base.SetInstantField();
        foreach (var item in DataManager.Instance.playerData.handEntities)
        {
            var entity = ResourceManager.CreateEntity(item.name);
            PlaceEntity(entity, instantField);
        }
    }
    public override void SetMainField()
    {
        base.SetMainField();
        foreach (var item in DataManager.Instance.playerData.fieldEntities)
        {
            var entity = ResourceManager.CreateEntity(item.Value.name);

            // key를 좌표 값으로 전환
            intVector2 vec = intVector2.Decode(item.Key);
            PlaceEntity(entity, GameManager.Instance.field, vec);
        }
    }

    public override void DisposeInstantField()
    {

        // 추가로 배치한 기물을 데이터에 업데이트
        var tiles = GameManager.Instance.field.GetHalfTiles(isReflect);
        Dictionary<int, EntityData> fieldData = new Dictionary<int, EntityData>();
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            var entityData = new EntityData(entity);
            int pos = tile.fieldPos.Encode();
            fieldData.Add(pos, entityData);
            Debug.Log($"data {entityData.name} : pos {pos}");
        }
        DataManager.Instance.playerData.fieldEntities = fieldData;

        // 인스턴트 필드에 남은 기물을 데이터에 업데이트
        List<EntityData> handData = new List<EntityData>();
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            var entityData = new EntityData(entity);
            handData.Add(entityData);
        }
        DataManager.Instance.playerData.handEntities = handData;
        base.DisposeInstantField();
    }
}
