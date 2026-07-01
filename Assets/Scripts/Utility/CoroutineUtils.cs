using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtils
{
    public static Coroutine RunWithCallback(this MonoBehaviour runner, IEnumerator coroutine, Action onComplete)
    {
        return runner.StartCoroutine(Wrapper(coroutine, onComplete));
    }

    public static Coroutine RunWithCallback(this MonoBehaviour runner, List<IEnumerator> coroutines, Action onComplete)
    {
        return runner.StartCoroutine(ListWrapper(runner, coroutines, onComplete));
    }


    private static IEnumerator Wrapper(IEnumerator coroutine, Action onComplete)
    {
        yield return coroutine;
        onComplete?.Invoke();
    }

    private static IEnumerator ListWrapper(MonoBehaviour runner, List<IEnumerator> coroutines, Action onComplete)
    {
        if (coroutines == null || coroutines.Count == 0)
        {
            onComplete?.Invoke();
            yield break;
        }
        
        int remainCount = coroutines.Count;

        foreach (var coroutine in coroutines)
        {
            runner.StartCoroutine(Wrapper(coroutine, () => remainCount--));
        }
        
        yield return new WaitUntil(() => remainCount <= 0);
        
        onComplete?.Invoke();
    }
}
