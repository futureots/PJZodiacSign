using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Command
{
    public event Action onDestroyed;

    /// <summary>
    /// 커맨드 실행
    /// </summary>
    public virtual IEnumerator Execute(Action callback = null)
    {
        callback?.Invoke();
        Delete();
        yield break;
    }

    public void Delete()
    {
        onDestroyed?.Invoke();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    
}
