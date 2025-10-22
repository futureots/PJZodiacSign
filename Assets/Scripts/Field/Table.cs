using System.Collections.Generic;
using UnityEngine;


public abstract class Table<T> : ScriptableObject where T : AbstractData
{
    [SerializeField] protected List<T> tableData;
    public List<T> table
    {
        get
        {
            return tableData;
        }
    }

    public T SearchData(string id)
    {
        var item = table.Find(x => x.id.Equals(id));
        //if (item == null) return null;
        return item;
    }
}
