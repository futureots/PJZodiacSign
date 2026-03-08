using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class S_Castling : BaseSkillLogic
{
    private Entity _entity;
    private Entity _target;

    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        Entity entity = null;
        if(component.TryGetComponent<Entity>(out var owner))
        {
            entity = owner;
        }
        else
        {
            var data = await input.InputEntity(list, 1);
            if(data != null)
            {
                entity = data[0];
            }
            else
            {
                return false;
            }
        }
        list.Remove(entity);
        
        var data2 = await input.InputEntity(list, 1);
        if (data2 == null) 
        {
            return false;
        }
        _entity =  entity;
        _target = data2[0];
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        var tile1 = _entity.CurTile;
        var tile2 = _target.CurTile;
        tile1.UnsetOccupant();
        tile2.UnsetOccupant();
        yield return null;
        _target.Move(tile1);
        _entity.Move(tile2);
        
        _entity = null;
        _target = null;

        yield return null;
        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Castling();
    }
}
