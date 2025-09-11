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
        DeleteObjects();
    }

    public virtual void DeleteObjects()
    {
        //Debug.Log($"Selecter : {selecterObjects.Count}");
        foreach (GameObject go in selecterObjects)
        {
            GameObject.Destroy(go);
        }
        selecterObjects.Clear();
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
