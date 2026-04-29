using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_PowerDown : BaseSkillLogic
{
    private Entity _owner;
    [SerializeField] private Area area;

    public void Init(Area area)
    {
        this.area = area;
    }
    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            _owner = entity;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _owner = data[0];
            return true;
        }
        
        return false;

    }

    public override IEnumerator ExecuteSkill()
    {
        
        var pos = _owner.CurTile.fieldPos;
        int[,] t = new int[8, 8];
        var vectors = area.GetVectors(t, pos, _owner.direction);
        var tiles = StageManager.Instance.field.GetTiles(vectors);
        
        EffectFactory.Instance.Request("PowDownAura",_owner.transform.position,_owner.transform.lossyScale*3);
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (!entity.team.IsAlly(_owner.team))
                {
                    entity.Power -= 1;
                }
            }
        }
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_PowerDown();
        clone.Init(area);
        return clone;
    }
}
