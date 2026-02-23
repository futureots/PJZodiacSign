using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IInput
{


    public UniTask<List<Entity>> InputEntity(List<Entity> list,int count = -1);
    
    public UniTask<List<Tile>> InputTile(List<Tile> list,int count = -1);
    
}
