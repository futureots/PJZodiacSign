using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectFactory : Singleton<EffectFactory>
{
    // Effect table
    public EffectTable dataTable;
    private Dictionary<string, GameObject> _dataDictionary = new();
    
    // Pool Map
    private Transform _poolFolder;
    [SerializeField] private int poolCount = 1;
    [SerializeField] private int maxPoolCount = 10;
    
    private readonly Dictionary<string, IObjectPool<GameObject>> _poolMap = new();

    private void Start()
    {
        Init(dataTable);
    }
    
    /// Preset Table and Pool
    private void Init(EffectTable table)
    {
        // Get Table
        dataTable = table;
        _dataDictionary = dataTable.GetTableDictionary();
        
        // Set pool
        SetPool();
    }
    
    /// <summary>
    /// Get New Effect Object
    /// </summary>
    /// <param name="objName">Effect Name</param>
    /// <param name="position">Invoke Position</param>
    /// <param name="scale">Effect Size</param>
    /// <param name="time">Duration</param>
    /// <returns>Effect Object : GameObject</returns>
    public GameObject Request(string objName, Vector3 position, Vector3 scale, float time =2f)
    {
        EnsurePool(objName);
        
        // Get Effect from Pool
        GameObject obj = _poolMap[objName].Get();
        
        // Set Effect
        obj.transform.position = position;
        obj.transform.localScale = scale;
        
        // return after "time" sec.
        StartCoroutine(ActEffect(objName, obj, time));
        return obj;
    }
    
    /// Effect Action (duration
    private IEnumerator ActEffect(string objName, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if (obj && obj.activeSelf)
        {
            _poolMap[objName].Release(obj);
        }
    }
    
    #region PoolConfig

    private void SetPool()
    {
        // Create Pool Folder
        if (!_poolFolder)
        {
            var newFolder = new GameObject("EffectPool");
            newFolder.transform.SetParent(transform);
            _poolFolder = newFolder.transform;
        }
        
        // CreatePool
        foreach (var item in _dataDictionary)
        {
            if (!_poolMap.ContainsKey(item.Key))
            {
                EnsurePool(item);
            }
            Fill(item.Key, poolCount);
        }
    }

    private void EnsurePool(string objName)
    {
        if (!_poolMap.ContainsKey(objName)) return;
        KeyValuePair<string, GameObject> pair = new(objName, _dataDictionary[objName]);
        EnsurePool(pair);
    }
    
    private void EnsurePool(KeyValuePair<string, GameObject> data)
    {
        if (_poolMap.ContainsKey(data.Key)) return;
        
        _poolMap[data.Key] = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var go = Instantiate(data.Value, _poolFolder);
                go.name = data.Key;
                return go;
            },
            actionOnGet: o => o.SetActive(true),
            actionOnRelease: o => o.SetActive(false),
            actionOnDestroy:  o => Destroy(o.gameObject),
            defaultCapacity:  poolCount,
            maxSize: maxPoolCount
            );
    }

    private void Fill(string key, int count)
    {
        if (count <= 0) return;

        List<GameObject> objects = new();
        for (int i = 0; i < count; i++)
        {
            objects.Add(_poolMap[key].Get());
        }

        foreach (var item in objects)
        {
            _poolMap[key].Release(item);
        }
    }
    
    #endregion
}
