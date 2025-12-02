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
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance.transform.root.gameObject);
         
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