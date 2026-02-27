using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Suicide : BaseSkillLogic
{
    [SerializeField] private Area area;
    private Entity _target;
    
    public void Init(Area area)
    {
        this.area = area;
    }
    
    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _target = data[0];
            return true;
        }
        
        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var pos = _target.CurTile.fieldPos;
        int[,] t = new int[8, 8];
        var vectors = area.GetVectors(t, pos, _target.direction);
        var tiles = StageManager.Instance.field.GetTiles(vectors);
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            if (tile.occupiedObject.TryGetComponent<Entity>(out var entity))
            {
                entity.Damaged(_target.Power);
            }
        }
        _target = null;

        yield return new WaitForSeconds(0.5f);
    }

    public override BaseSkillLogic Clone()
    {
        S_Suicide clone = new();
        clone.Init(area);
        return clone;
    }
}
