using UnityEngine;

public class EnemyController : EntityController
{
    public override void DisposeInstantField()
    {
        foreach(var tile in instantField.tiles)
        {
            if (tile.isEmpty) continue;
            Debug.Log("Occupied");
            var entity = tile.occupiedObject.GetComponent<Entity>();

            entity.MoveSequence(GameManager.Instance.field.GetTile(tile.fieldPos, isReflect), true);
        }
        base.DisposeInstantField();
    }
}
