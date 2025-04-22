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

    private static IEnumerator Wrapper(IEnumerator coroutine, Action onComplete)
    {
        yield return coroutine;
        onComplete?.Invoke();
    }
}
