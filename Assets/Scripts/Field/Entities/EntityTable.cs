using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityTable", menuName = "Scriptable Objects/EntityTable")]
public class EntityTable : ScriptableObject
{
    [SerializeField] List<EntityData> entityList;
    Dictionary<string, EntityData> _entities;
    public Dictionary<string, EntityData> entities
    {
        get
        {
            if(_entities == null)
            {
                _entities = new Dictionary<string, EntityData>();
                foreach (var entity in entityList)
                {
                    _entities.Add(entity.id, entity);
                }
            }
            return _entities;
        }
    }
     
}
