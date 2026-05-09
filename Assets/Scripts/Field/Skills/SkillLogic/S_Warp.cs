using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


[Serializable]
public class S_Warp : BaseSkillLogic
{
    private Entity _owner;
    private Tile _tile;
    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        if(component.TryGetComponent<Entity>(out var owner)) { }
        else
        {
            var data = await input.InputEntity(list, 1);
            if(data != null)
            {
                owner = data[0];
            }
            else
            {
                return false;
            }
        }
        
        var tiles = StageManager.Instance.field.GetTiles().Where(tile => tile.IsEmpty).ToList();
        var data2 = await input.InputTile(tiles, 1);
        if (data2 == null) 
        {
            return false;
        }
        
        _owner = owner;
        _tile = data2[0];
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        _owner.gameObject.transform.DOMove(_tile.transform.position,1f);
        yield return new WaitForSeconds(1f);
        _owner.Move(_tile);
        _tile = null;
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Warp();
    }
}
