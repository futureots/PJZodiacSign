using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Suicide : BaseSkillLogic
{
    [SerializeField] private Area area;
    private Entity _owner;
    
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
        // TODO : 폭발 이펙트 재생
        EffectFactory.Instance.Request("ExplodeEffect",_owner.transform.position,_owner.transform.lossyScale*5);
        
        foreach (var tile in tiles)
        {
            if (tile.IsEmpty) continue;
            tile.occupiedEntity.Damaged(_owner.MaxHealth);
        }
        _owner.Damaged(_owner.CurHealth - 1);
        _owner = null;
        yield return new WaitForSeconds(1);
    }

    public override BaseSkillLogic Clone()
    {
        S_Suicide clone = new();
        clone.Init(area);
        return clone;
    }
}
