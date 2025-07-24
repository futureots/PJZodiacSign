using UnityEngine;

public class ResourceManager
{
    //static readonly string EntityPath = "Prefabs/Entity/";
    static readonly string TablePath = "Tables/BaseEntityTable";
    
    public static EntityData GetEntityResource(string entityId)
    {
        var obj = Resources.Load<EntityTable>(TablePath);
        var data = obj.entities[entityId];
        return data;
    }

    public static Entity CreateEntity(string entityName, int level = 0)
    {
        var resource = GetEntityResource(entityName);
        if(resource == null)
        {
            Debug.Log($"{entityName} is Not Exist");
            return null;
        }
        var instance = resource.CreateEntity(level);
        return instance;
    }
}
