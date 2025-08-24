using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Command
{

    public List<GameObject> selecterObjects;
    /// <summary>
    /// 커맨드 실행
    /// </summary>
    public virtual void Execute()
    {
        Delete();
    }

    public virtual void Delete()
    {
        Debug.Log($"Selecter : {selecterObjects.Count}");
        foreach (GameObject go in selecterObjects)
        {
            UnityEngine.Object.Destroy(go);
        }
        selecterObjects.Clear();
    }
}
