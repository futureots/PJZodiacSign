using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public override void SetMainField(Dictionary<int, EntityData> fieldEntities)
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
    public override void DisposeInstantField(ref Dictionary<int, EntityData> fields, ref List<EntityData> hands)
    {
        base.DisposeInstantField(ref fields, ref hands);
    }
}
