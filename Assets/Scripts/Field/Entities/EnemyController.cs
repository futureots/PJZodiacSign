using System.Collections.Generic;
using UnityEngine;

using static UnityEditor.Progress;

public class EnemyController : EntityController
{
    public override void SetMainField(Dictionary<int, EntityLevelData> fieldEntities)
    {
        base.SetMainField(fieldEntities);
        foreach (var datum in fieldEntities)
        {
            var entity = ResourceManager.CreateEntity(datum.Value.entity,datum.Value.level);

            // key를 좌표 값으로 전환
            intVector2 vec = intVector2.Decode(datum.Key);
            PlaceEntity(entity, GameManager.Instance.field);
            
        }
        
    }
    public override void DisposeInstantField()
    {
        Debug.Log(name + " DisposeInstantField");
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            Debug.Log("Enemy OBject : " + entity);
            PlaceEntity(entity, GameManager.Instance.field);
        }
        
        base.DisposeInstantField();
        
    }
}
