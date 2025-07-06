using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityTable", menuName = "Scriptable Objects/EntityTable")]
public class EntityTable : ScriptableObject
{
    public List<EntityStep> entityTable;

    public string GetEntityData(int index, ref int cost)
    {
        int value = cost;
        var step = entityTable[index];
        var list = step.entities.Where(x => x.cost < value).ToList();
        Debug.Log(list.Count);
        if (list.Count > 0)
        {
            var rand = Random.Range(0, list.Count);
            cost -= list[rand].cost;
            return list[rand].name;
        }
        else return null;
    }

}
[System.Serializable]
public struct EntityStep
{
    // 같은 고려단계에 있는 엔티티들
    public List<EntityDefinition> entities;
}
[System.Serializable]
public struct EntityDefinition
{

    public string name;
    public int cost;

    public EntityDefinition(string name, int cost)
    {
        this.name = name;
        this.cost = cost;
    }
}