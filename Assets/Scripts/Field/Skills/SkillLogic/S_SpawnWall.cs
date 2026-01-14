using System;
using System.Collections;


[Serializable]
public class S_SpawnWall : BaseSkillLogic
{
    Entity owner;
    Tile tile;
    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        isContinued = false;
        Action<bool> conti = (flag) => { isContinued = flag; };
        if (component.TryGetComponent<Entity>(out var _owner))
        {

        }
        else
        {
            var list = StageManager.Instance.field.GetEntities();
            _owner = null;
            Action<Entity> action = (x) =>
            {
                _owner = x;
            };
            yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
            if (!isContinued)
            {
                callback?.Invoke(false);
                yield break;
            }
        }

        var tiles = _owner.GetAttackArea();
        Tile _tile = null;
        Action<Tile> action2 = (x) =>
        {
            _tile = x;
        };
        yield return component.StartCoroutine(input.InputTile(tiles, action2, conti, 1));
        if (!isContinued)
        {
            callback?.Invoke(false);
            yield break;
        }


        owner = _owner;
        tile = _tile;
        callback?.Invoke(true);
        yield break;
    }

    public override IEnumerator ExecuteSkill()
    {
        // TODO : 팩토리를 통해 장애물을 생성하고 tile에 생성
        tile = null;
        owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_SpawnWall();
    }
}
