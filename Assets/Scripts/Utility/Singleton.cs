using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour                     
{
    /**
     * Singleton Instance
     * - If Exist, Return/Destroy dup.
     * - If not Exist, return Null
     */
    private static T instance = null;
    private static bool _isQuitting = false;
    private static readonly object _lock = new();
    
    public static T Instance                                                                                           
    {
        get
        {
            // 1. On Quitting
            if (_isQuitting)
            {
                return null;
            }

            // LOCK : Access Instance
            lock (_lock)
            {
                if (!instance)
                {
                    // Check Object Exists
                    instance = (T)FindAnyObjectByType(typeof(T));
                }

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

public class SingletonObject<T> : Singleton<T> where T : MonoBehaviour                     
{
    /**
     * DDOL Singleton
     */
    
    protected override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(gameObject);
    }
}