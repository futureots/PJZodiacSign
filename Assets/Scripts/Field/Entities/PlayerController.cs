using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : EntityController
{

    public override void SetMainField(Dictionary<int,EntityLevelData> fieldEntities)
    {
        base.SetMainField(fieldEntities);
        
        foreach (var item in fieldEntities)
        {
            var entity = ResourceManager.CreateEntity(item.Value.entity,item.Value.level);

            // key를 좌표 값으로 전환
            intVector2 vec = intVector2.Decode(item.Key);
            PlaceEntity(entity, GameManager.Instance.field, vec);
        }
    }

    public override void DisposeInstantField()
    {
        base.DisposeInstantField();
    }
}
