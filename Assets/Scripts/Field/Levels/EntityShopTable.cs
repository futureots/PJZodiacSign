using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityShopTable", menuName = "Scriptable Objects/EntityShopTable")]
public class EntityShopTable : ScriptableObject
{
    [SerializeField] List<EntityCost> table;
    /// <summary>
    /// 해당 엔티티 가격 반환
    /// </summary>
    /// <param name="name">엔티티 이름</param>
    /// <returns>엔티티 가격</returns>
    public int GetEntityCost(string name)
    {
        var index = table.FindIndex((x) => x.entityName == name);
        if(index != -1)
        {
            return table[index].cost;
        }
        else
        {
            return -1;
        }
        
    }

}
[System.Serializable]
public struct EntityCost
{
    public string entityName;
    public int cost;
}