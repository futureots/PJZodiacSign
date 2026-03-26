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
    private Transform poolFolder;
    [SerializeField] private int poolCount = 1;
    [SerializeField] private int maxPoolCount = 10;
    
    private Dictionary<string, IObjectPool<GameObject>> poolMap = new();

    private void Start()
    {
        Initialize(dataTable);
    }

    private void Initialize(EffectTable table)
    {
        // Get Table
        dataTable = table;
        _dataDictionary = dataTable.GetTableDictionary();
        
        // Set pool
        SetPool();
    }
    
    public GameObject Request(string objName, Vector3 position, Vector3 scale, float time =2f)
    {
        EnsurePool(objName);
        
        // Get Effect from Pool
        GameObject obj = poolMap[objName].Get();
        
        // Set Effect
        obj.transform.position = position;
        obj.transform.localScale = scale;
        
        // return after "time" sec.
        StartCoroutine(ActEffect(objName, obj, time));
        return obj;
    }

    private IEnumerator ActEffect(string objName, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if (obj != null && obj.activeSelf)
        {
            poolMap[objName].Release(obj);
        }
    }
    
    #region PoolConfig

    private void SetPool()
    {
        // Create Pool Folder
        if (!poolFolder)
        {
            var newFolder = new GameObject("Pool");
            newFolder.transform.SetParent(transform);
            poolFolder = newFolder.transform;
        }
        
        // CreatePool
        foreach (var item in _dataDictionary)
        {
            if (!poolMap.ContainsKey(item.Key))
            {
                EnsurePool(item);
            }
            Fill(item.Key, poolCount);
        }
    }

    private void EnsurePool(string objName)
    {
        if (!poolMap.ContainsKey(objName)) return;
        KeyValuePair<string, GameObject> pair = new(objName, _dataDictionary[objName]);
        EnsurePool(pair);
    }
    
    private void EnsurePool(KeyValuePair<string, GameObject> data)
    {
        if (poolMap.ContainsKey(data.Key)) return;
        
        poolMap[data.Key] = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var go = Instantiate(data.Value, poolFolder);
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
            objects.Add(poolMap[key].Get());
        }

        foreach (var item in objects)
        {
            poolMap[key].Release(item);
        }
    }
    
    #endregion
}
