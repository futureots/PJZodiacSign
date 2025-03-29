using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : EntityController
{
    public override void SetEntities(int num, PartyData party)
    {
        base.SetEntities(num, party);
        var tiles = currentField.GetHalfTiles(isReflect);
        foreach (var entity in entities)
        {
            entity.tag = "Enemy";
            var tile = tiles[Random.Range(0, tiles.Count)];
            tiles.Remove(tile);
            entity.MoveToTile(tile,false);
        }
    }
}
