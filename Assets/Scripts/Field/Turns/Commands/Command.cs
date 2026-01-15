using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public interface IExecute
{
    public IEnumerator Execute(Action callback);
}

public record Command : IExecute
{
    /**
     * 명령, 실행 별 단위 행동 데이터
     * - 상속받은 객체별로 인자 데이터를 이용한 작업 수행
     */

    public List<GameObject> indicate = new();
    
    public virtual IEnumerator Execute(Action callback = null)
    {
        callback?.Invoke();
        Delete();
        yield break;
    }

    public void Delete()
    {
        foreach (var obj in indicate)
        {
            Object.Destroy(obj);
        }
    }

    public override string ToString()
    {
        return $"{typeof(Command)}";
    }
}
