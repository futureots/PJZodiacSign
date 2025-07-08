using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StageEntityTable", menuName = "Scriptable Objects/StageEntityTable")]
public class StageEntityTable : ScriptableObject
{
    public EntityShopTable shopTable;
    [SerializeField] List<EntityPriority> priority;

    public string GetRandomEntity(int cost)
    {
        int temp = cost;
        var list = priority.Where(x => shopTable.GetEntityCost(x.name) <= temp);
        if (list.Count() <= 0) return "";
        var sum = list.Sum(x => x.priority);
        Debug.Log(sum);
        var rand = Random.Range(0, sum);
        foreach (var item in list)
        {
            rand -= item.priority;
            if(rand < 0)
            {
                
                return item.name;
            }
        }
        return list.Last().name;

    }
}
[System.Serializable]
public struct EntityPriority
{
    public string name;
    public int priority;
}
