using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_HealArea : AreaSkillLogic
{
    [SerializeField] private int multiplier = 1;
    private Entity _owner;
    public void Init(Area area, int multiplier)
    {
        this.area = area;
        this.multiplier = multiplier;
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

    public override bool IsValuable()
    {
        if (component.TryGetComponent<Entity>(out var owner))
        {
            _owner = owner;
            
            var pos = _owner.CurTile.fieldPos;
            int[,] t = new int[8, 8];
            var vectors = area.GetVectors(t, pos, _owner.direction);
            var tiles = StageManager.Instance.field.GetTiles(vectors);

            // 아군이 있으면 사용
            return tiles.Exists(tile => !tile.IsEmpty && tile.occupiedEntity.team.IsAlly(_owner.team));
        }

        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var pos = _owner.CurTile.fieldPos;
        int[,] t = new int[8, 8];
        var vectors = area.GetVectors(t, pos, _owner.direction);
        var tiles = StageManager.Instance.field.GetTiles(vectors);
        
        EffectFactory.Instance.Request("HealAura", _owner.transform.position, Vector3.Scale(_owner.transform.lossyScale,new Vector3(5,1,5)));
        yield return new WaitForSeconds(0.1f);
        
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.team.IsAlly(_owner.team))
            {
                Entity entity = tile.occupiedEntity;
                entity.Healed(_owner.Power*multiplier);
            }
        }
        
        yield return new WaitForSeconds(0.9f);
        
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_HealArea();
        clone.Init(area,multiplier);
        return clone;
    }
}
