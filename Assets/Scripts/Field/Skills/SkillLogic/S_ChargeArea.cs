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
    public void Init(Area _area)
    {
        area = _area;
    }

    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        isContinued = false;
        if (component.TryGetComponent<Entity>(out var _owner))
        {
            owner = _owner;
            callback?.Invoke(true);
            yield break;
        }
        else
        {
            var list = StageManager.Instance.field.GetEntities();
            Entity _target = null;
            Action<Entity> action = (x) =>
            {
                _target = x;
            };
            Action<bool> conti = (flag) => { isContinued = flag; };
            yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
            if (!isContinued)
            {
                callback?.Invoke(false);
                yield break;
            }
            owner = _target;
            callback?.Invoke(true);
        }

    }

    public override IEnumerator ExecuteSkill()
    {
        var pos = owner.CurTile.fieldPos;
        int[,] t = new int[8, 8];
        var vectors = area.GetVectors(t, pos, owner.IsReflect);
        var tiles = StageManager.Instance.field.GetTiles(vectors);
        foreach (var tile in tiles)
        {
            if (tile.occupiedObject.TryGetComponent<EnergyComponent>(out var energy))
            {
                energy.CurEnergy += charge;
            }
        }
        owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_ChargeArea();
        clone.Init(area);
        return clone;
    }
}
