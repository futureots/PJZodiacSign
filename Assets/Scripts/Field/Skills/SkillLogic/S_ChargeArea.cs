using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_ChargeArea : BaseSkillLogic
{
    [SerializeField] int charge;
    [SerializeField] Area area;
    Entity owner;
    public S_ChargeArea() { }
    public void Init(Area _area, int amount)
    {
        area = _area;
        charge = amount;
    }

    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var _owner))
        {
            owner = _owner;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            owner = data[0];
            return true;
        }
        
        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var pos = owner.CurTile.fieldPos;
        int[,] t = new int[8, 8];
        var vectors = area.GetVectors(t, pos, owner.direction);
        var tiles = StageManager.Instance.field.GetTiles(vectors);
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            if(tile.occupiedObject.TryGetComponent<Entity>(out var entity))
            {
                if (entity.team.IsAlly(owner.team))
                {
                    entity.energy.CurEnergy += charge;
                }
            }
        }
        owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_ChargeArea();
        clone.Init(area,charge);
        return clone;
    }
}
