using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    // TODO: Pooling 용 함수


    /// <summary>
    /// Request for New Entity
    /// </summary>
    /// <param name="data">Entity Data to make</param>
    /// <param name="level">Init with level</param>
    /// <param name="tile">position for new Entity</param>
    /// <returns>Entity Object to Get</returns>
    public static Entity RequestEntity(EntityData data, int level = 0, Tile tile = null)
    {
        var entity = Instantiate(data.prefab);
        
        // 기초 스탯 적용
        entity.name = data.id;
        entity.Init(data, level);

        // Move to Initial Tile
        if (!tile)
        {
            entity.Move(tile, true);
        }

        return entity;
    }
}
