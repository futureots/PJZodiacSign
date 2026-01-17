using System;
using System.Collections;

[Serializable]
public class S_Promotion : BaseSkillLogic
{
    Entity entity;
    Entity target;
    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        var list = StageManager.Instance.field.GetEntities();
        isContinued = false;
        Entity _entity = null;
        Action<bool> conti = (flag) => { isContinued = flag; };
        if(component.TryGetComponent<Entity>(out var _owner))
        {
            _entity = _owner;
        }
        else
        {
            Action<Entity> action = (x) =>
            {
                _entity = x;
            };
            yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
            if (!isContinued)
            {
                callback?.Invoke(false);
                yield break;
            }
            
        }
        list.Remove(_entity);


        Entity _target = null;
        Action<Entity> action2 = (x) =>
        {
            _target = x;
        };
        yield return component.StartCoroutine(input.InputEntity(list, action2, conti, 1));
        if (!isContinued)
        {
            callback?.Invoke(false);
            yield break;
        }
        target = _target;
        entity = _entity;
        callback?.Invoke(true);
    }

    public override IEnumerator ExecuteSkill()
    {
        var data = target.baseData;
        var dir = entity.direction;
        var tile = entity.CurTile;
        var level = entity.Level;
        var team = entity.team.teamNumber;
        entity.CurTile.UnsetOccupant();
        entity.Dead();
        entity = null;
        // TODO : entity의 data를 변경하고 팩토리를 통해 새로 생성, entity의 레벨은 유지
        var promotion = EntityFactory.RequestEntity(target.baseData, dir, tile, level);
        promotion.team.teamNumber = team;
        target = null;
        
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Promotion();
    }
}
