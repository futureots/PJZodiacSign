using System.Collections.Generic;
using UnityEngine;

using static UnityEditor.Progress;

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
        foreach (var datum in fieldEntities)
        {
            var entity = ResourceManager.CreateEntity(datum.Value.entity);

            // key를 좌표 값으로 전환
            intVector2 vec = intVector2.Decode(datum.Key);
            PlaceEntity(entity, GameManager.Instance.field);
            
        }
        
    }
    public override void DisposeInstantField(ref Dictionary<int, EntityLevelData> fields, ref List<EntityLevelData> hands)
    {
        Debug.Log(name + " DisposeInstantField");
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            Debug.Log("Enemy OBject : " + entity);
            PlaceEntity(entity, GameManager.Instance.field);
        }
        
        base.DisposeInstantField(ref fields, ref hands);
        
    }
}
