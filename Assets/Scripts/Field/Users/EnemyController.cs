using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{

    public override void DisposeInstantField()
    {
        Debug.Log(name + " DisposeInstantField");
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            Debug.Log("Enemy OBject : " + entity);
            //PlaceEntity(entity, GameManager.Instance.field);
        }
        
        base.DisposeInstantField();
        
    }
}
