using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public override void SetInstantField()
    {
        base.SetInstantField();
        // 나중에 임시데이터 말고 스테이지 별로 로직 추가하기
        foreach (var item in DataManager.Instance.playerData.handEntities)
        {
            var entity = ResourceManager.CreateEntity(item.name);
            PushEntity(entity, instantField);
        }
    }
    public override void SetMainField()
    {
        base.SetMainField();
        foreach (var tile in instantField.tiles)
        {
            if (tile.isEmpty) continue;
            Debug.Log("Occupied");
            var entity = tile.occupiedObject.GetComponent<Entity>();

            entity.MoveSequence(GameManager.Instance.field.GetTile(tile.fieldPos, isReflect), true);
        }
        
    }
}
