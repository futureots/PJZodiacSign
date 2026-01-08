using System;
using System.Collections;
using System.Collections.Generic;

public class RandomInput : IInput
{
    public IEnumerator InputEntity(List<Entity> list, Action<Entity> input, Action<bool> callback, int count = -1)
    {
        int min = Math.Min(list.Count, count);
        for (int i = 0; i < min; i++)
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            var value = list[rand];
            list.RemoveAt(rand);
            input?.Invoke(value);
        }
        callback?.Invoke(true);
        yield break;
    }

    public IEnumerator InputTile(List<Tile> list, Action<Tile> input, Action<bool> callback, int count = -1)
    {
        int min = Math.Min(list.Count, count);
        for (int i = 0; i < min; i++)
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            var value = list[rand];
            list.RemoveAt(rand);
            input?.Invoke(value);
        }
        callback?.Invoke(true);
        yield break;
    }
}
