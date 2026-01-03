using System;
using System.Collections;
using System.Collections.Generic;

public interface IInput
{
    public IEnumerator InputEntity(List<Entity> list, Action<Entity> input, Action<bool> callback, int count = -1);

    public IEnumerator InputTile(List<Tile> list, Action<Tile> input, Action<bool> callback, int count = -1);
}
