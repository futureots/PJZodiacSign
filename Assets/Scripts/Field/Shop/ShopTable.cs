using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopTable", menuName = "Scriptable Objects/ShopTable")]
public class ShopTable : ScriptableObject
{
    public List<ItemData> itemList;
    public List<EntityUIData> entityList;


    public List<ItemData> GetRandomItem(int count)
    {
        List<ItemData> list = new List<ItemData>();
        // 리스트 순회해서 랜덤 아이템 데이터 반환하기
        foreach (ItemData item in itemList)
        {
            list.Add(item);
        }
        return list;
    }
    public List<EntityUIData> GetRandomEntity(int count)
    {
        List<EntityUIData> list = new List<EntityUIData>();
        // 리스트 순회해서 랜덤 아이템 데이터 반환하기
        foreach (var item in entityList)
        {
            list.Add(item);
        }
        return list;
    }
}
