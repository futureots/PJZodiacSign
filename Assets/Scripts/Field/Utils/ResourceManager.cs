using UnityEngine;

public class ResourceManager
{
    static readonly string EntityPath = "Prefabs/Entity/";
    public static Entity GetEntityResource(string entityName)
    {
        var obj = Resources.Load<Entity>(EntityPath + entityName);
        return obj;
    }

    public static Entity CreateEntity(string entityName)
    {
        var resource = GetEntityResource(entityName);
        if(resource == null)
        {
            Debug.Log($"{entityName} is Not Exist");
            return null;
        }
        var instance = GameObject.Instantiate(resource);
        var entity = instance.GetComponent<Entity>();
        return entity;
    }
}
