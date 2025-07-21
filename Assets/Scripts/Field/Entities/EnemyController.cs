using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public override void SetInstantField(List<EntityLevelData> handEntities)
    {
        base.SetInstantField(handEntities);
        foreach (var item in handEntities)
        {
            var entity = ResourceManager.CreateEntity(item.entity);
            PlaceEntity(entity, instantField);
        }
    }
    public override void SetMainField(Dictionary<int, EntityLevelData> fieldEntities)
    {
        base.SetMainField(fieldEntities);
        foreach (var tile in instantField.tiles)
        {
            if (tile.isEmpty) continue;
            Debug.Log("Occupied");
            var entity = tile.occupiedObject.GetComponent<Entity>();

            entity.MoveSequence(GameManager.Instance.field.GetTile(tile.fieldPos, isReflect), true);
        }
        
    }
    public override void DisposeInstantField(ref Dictionary<int, EntityLevelData> fields, ref List<EntityLevelData> hands)
    {
        base.DisposeInstantField(ref fields, ref hands);
    }
}
