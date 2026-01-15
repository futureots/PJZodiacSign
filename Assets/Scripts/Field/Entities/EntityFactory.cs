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
    public static Entity RequestEntity(EntityData data, intVector2 direction, Tile tile = null, int level = 0)
    {
        var entity = Instantiate(data.prefab);
        
        // 기초 스탯 적용
        entity.name = data.id;
        entity.Init(data, direction, level);

        // Move to Initial Tile
        if (tile)
        {
            entity.Move(tile, true);
        }
        entity.transform.localScale = Vector3.one;
        return entity;
    }
}

public class ItemFactory : MonoBehaviour
{
    public static ItemComponent RequestItem(ItemData data)
    {
        var item = Instantiate(data.prefab);
        item.Init(data);
        return item;
    }
}