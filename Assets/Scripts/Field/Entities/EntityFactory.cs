using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EntityFactory : Singleton<EntityFactory>
{
    // Pool Map
    [Header("Pool Config")] 
    private Transform _poolFolder;
    [SerializeField] private int poolCount = 5;
    [SerializeField] private int maxPoolSize = 30;
    
    private readonly Dictionary<EntityData, IObjectPool<Entity>> _poolMap = new();
    private readonly Dictionary<Entity, EntityData> _poolTicket = new();
    public event Action<Entity> OnEntityCreated;

    /// <summary>
    /// Get New Entity
    /// </summary>
    /// <param name="data">Entity Data to make</param>
    /// <param name="level">Init with level</param>
    /// <param name="direction">Direction for Entity init</param>
    /// <param name="tile">position for new Entity</param>
    /// <param name="team">entity's owner team</param>
    /// <returns>Entity Object to Get</returns>
    public Entity Request(EntityData data, intVector2 direction = new(), Tile tile = null, PlayerID team = PlayerID.None, int level = 0)
    {
        // Data Check for Exception
        if (!data) return null;
        EnsurePool(data);
        
        // Get Entity from Pool
        Entity entity = _poolMap[data].Get();
        entity.Init(data, direction, level);

        // Move to Initial Tile
        if (tile)
        {
            entity.Move(tile, true);
        }
        entity.transform.localScale = Vector3.one;
        OnEntityCreated?.Invoke(entity);
        
        return entity;
    }
    
    #region PoolConfig    
    
    /// <summary>
    /// Warm-up Pools for Entity
    /// </summary>
    /// <param name="entity">Entities for Pooling (Shop, Mimic, etc.)</param>
    /// <param name="agentEntity">Entities for Agents (Pool One)</param>
    public void SetPool(IEnumerable<EntityData> entity, IEnumerable<EntityLevelData> agentEntity = null)
    {
        // Make Pool Folder
        if (!_poolFolder)
        {
            _poolFolder = new GameObject("EntityPool").transform;
            _poolFolder.SetParent(transform);
        }
        
        // Check Count for Pool Queue
        Dictionary<EntityData, int> counts = new();

        if (agentEntity != null)
        {
            // Agent Entity : 1 each
            foreach (EntityLevelData e in agentEntity)
            {
                if (!e.data) continue;
                counts.TryAdd(e.data, 0);
                counts[e.data]++;
            }
        }

        // Field Entity(Shop) : Count
        foreach (var e in entity)
        {
            if (!e) continue;
            int current = counts.GetValueOrDefault(e);
            counts[e] = Mathf.Max(current, poolCount);
        }

        // Create Pool and Fill
        foreach (var pair in counts)
        {
            EnsurePool(pair.Key);
            Fill(pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// Check Pool Exists and Create
    /// </summary>
    /// <param name="data">Entity Info for Pooling</param>
    private void EnsurePool(EntityData data)
    {
        if (_poolMap.ContainsKey(data)) return;

        _poolMap[data] = new ObjectPool<Entity>(
            // CREATION
            createFunc: () =>
            {
                var e = Create(data);
                _poolTicket[e] = data;
                return e;
            },
            // Get Obj from Pool
            actionOnGet: Get,
            // Set to Pool
            actionOnRelease: Release,
            // Destroy
            actionOnDestroy: (e) =>
            {
                Destroy(e.gameObject);
            },
            defaultCapacity: poolCount,
            maxSize:  maxPoolSize
            );
    }

    // Create New Entity
    private Entity Create(EntityData data)
    {
        var e = Instantiate(data.prefab, _poolFolder);
        e.name = data.id;
        return e;
    }
    
    // Activate Entity
    private void Get(Entity entity)
    {
        // NOTE: Entity 활성 및 초기화
        entity.gameObject.SetActive(true);
    }
    
    // Release Entity
    private void Release(Entity entity)
    {
        // NOTE: 활성 중 특수 상태 초기화
        entity.gameObject.SetActive(false);
    }

    /// <summary>
    /// Preset to pool
    /// </summary>
    /// <param name="data">Entity info for Pooling</param>
    /// <param name="count">amount of Pooling</param>
    private void Fill(EntityData data, int count)
    {
        // Set Count
        if (count <= 0) return;
        
        List<Entity> entities = new();
        for (int i = 0; i < count; i++)
        {
            entities.Add(_poolMap[data].Get());
        }

        foreach (var entity in entities)
        {
            _poolMap[data].Release(entity);
        }
    }
    
    #endregion
}
