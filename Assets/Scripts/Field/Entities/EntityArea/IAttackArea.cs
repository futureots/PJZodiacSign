using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackArea
{
    public List<Tile> GetAttackArea(Tile tile);
}
