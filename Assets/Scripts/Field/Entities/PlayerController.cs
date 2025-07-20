using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : EntityController
{

    public override void SetMainField(Dictionary<int,EntityData> fieldEntities)
    {
        base.SetMainField(fieldEntities);
        foreach (var item in fieldEntities)
        {
            var entity = ResourceManager.CreateEntity(item.Value.name);

            // key를 좌표 값으로 전환
            intVector2 vec = intVector2.Decode(item.Key);
            PlaceEntity(entity, GameManager.Instance.field, vec);
        }
    }

    public override void DisposeInstantField(ref Dictionary<int,EntityData> fields, ref List<EntityData> hands)
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
        fields = fieldData;

        // 인스턴트 필드에 남은 기물을 데이터에 업데이트
        List<EntityData> handData = new List<EntityData>();
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            var entityData = new EntityData(entity);
            handData.Add(entityData);
        }
        hands = handData;
        base.DisposeInstantField(ref fields, ref hands);
    }
}
