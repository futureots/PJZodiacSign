using UnityEngine;

public class ResourceManager
{
    static readonly string EntityPath = "Prefabs/Entity/";
    public static Entity GetEntityResource(string entityName)
    {
        var obj = Resources.Load<Entity>(EntityPath + entityName);
        return obj;
    }
}
