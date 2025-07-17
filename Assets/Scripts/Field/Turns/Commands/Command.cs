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
        for (int i = selecterObjects.Count - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(selecterObjects[i]);
        }
        //Debug.Log("Command Delete");
    }
}
