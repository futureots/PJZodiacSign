using System;
using System.Collections;
using System.Linq;


[Serializable]
public class S_Warp : BaseSkillLogic
{
    Entity target;
    Tile tile;
    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        isContinued = false;
        Action<bool> conti = (flag) => { isContinued = flag; };

        var list = StageManager.Instance.field.GetEntities();
        Entity _target = null;
        Action<Entity> action = (x) =>
        {
            _target = x;
        };
        yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
        if (!isContinued)
        {
            callback?.Invoke(false);
            yield break;
        }

        var tiles = StageManager.Instance.field.GetTiles().Where(tile => tile.isEmpty).ToList();
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


        target = _target;
        tile = _tile;
        callback?.Invoke(true);
        yield break;
    }

    public override IEnumerator ExecuteSkill()
    {
        target.Move(tile);
        tile = null;
        target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_MoveAttack();
    }
}
