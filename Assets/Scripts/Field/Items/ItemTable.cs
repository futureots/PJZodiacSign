using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Objects/ItemTable")]
public class ItemTable : Table<ItemData>
{
    /*[SerializeField] List<ItemData> itemTableData;
    public List<ItemData> itemTable
    {
        get
        {
            return itemTableData;
        }
    }

    public ItemData SearchItem(string name)
    {
        var item = itemTable.Find(x => x.id.Equals(name));
        //Debug.Log(item.itemData.itemName);
        if (item == null) return null;
        return item;
    }*/
}

