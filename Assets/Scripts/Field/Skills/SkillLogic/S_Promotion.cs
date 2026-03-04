using Cysharp.Threading.Tasks;
using System;
using System.Collections;

[Serializable]
public class S_Promotion : BaseSkillLogic
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
        var data = _target.baseData;
        var dir = _entity.direction;
        var tile = _entity.CurTile;
        var level = _entity.Level;
        var team = _entity.team.teamNumber;
        
        _entity.CurTile.UnsetOccupant();
        _entity.Dead();
        _entity = null;
        
        // TODO : entity의 data를 변경하고 팩토리를 통해 새로 생성, entity의 레벨은 유지
        var promotion = EntityFactory.Instance.RequestEntity(target.baseData, dir, tile, level);
        promotion.team.teamNumber = team;

        _entity = null;
        _target = null;
        
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Promotion();
    }
}
