using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour                     
{
    private static T instance = null;
    public static T Instance                                                                                           
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
                if( instance is Singleton<T> singleton)
                {
                    singleton.Init();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        /*if (transform.parent != null || transform.root != null)                                                             
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }*/
    }

    public virtual void Init()
    {

    }
}

public class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour                     
{
    private static T instance = null;
    public static T Instance                                                                                           
    {
        get
        {
            if (!instance)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if (!instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}