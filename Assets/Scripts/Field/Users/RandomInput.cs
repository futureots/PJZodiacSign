using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class RandomInput : IInput
{
    public UniTask<List<Entity>> InputEntity(List<Entity> list, int count = -1)
    {
        if (list.Count <= 0) return new UniTask<List<Entity>>(null);
        if (list.Count <= count) return UniTask.FromResult(new List<Entity>(list));
        List<Entity> result = new List<Entity>();
        for (int i = 0; i < count; i++)
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            var value = list[rand];
            list.RemoveAt(rand);
            result.Add(value);
        }

        return UniTask.FromResult(result);
    }
    
    public UniTask<List<Tile>> InputTile(List<Tile> list, int count = -1)
    {
        if (list.Count <= 0) return new UniTask<List<Tile>>(null);
        if (list.Count <= count) return UniTask.FromResult(new List<Tile>(list));
        List<Tile> result = new List<Tile>();
        for (int i = 0; i < count; i++)
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            var value = list[rand];
            list.RemoveAt(rand);
            result.Add(value);
        }

        return UniTask.FromResult(result);
    }
}
