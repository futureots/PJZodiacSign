using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigEntity : Entity
{

    public override List<intVector2> GetMoveArea(intVector2 entityPos, bool hasOriginTile = false)
    {
        List<intVector2> area = new List<intVector2>();
        area.Add(new intVector2());
        area.Add(new intVector2());
        area.Add(new intVector2());
        area.Add(new intVector2());

        return base.GetMoveArea(entityPos, hasOriginTile);
    }

    public override List<intVector2> GetAttackArea(intVector2 entityPos)
    {
        return base.GetAttackArea(entityPos);
    }
}
