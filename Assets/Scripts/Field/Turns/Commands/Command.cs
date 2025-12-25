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

    // 시각화에 사용된 오브젝트 삭제(다른데로 이전해야함)
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

    public virtual bool AddObjects(params GameObject[] list)
    {
        if (selecterObjects == null) return false;
        selecterObjects.AddRange(list);
        return true;
    }
    
}
