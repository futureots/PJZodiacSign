using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class S_SpawnWall : BaseSkillLogic
{
    public EntityData spawnData;
    Tile tile;
    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        isContinued = false;
        Action<bool> conti = (flag) => { isContinued = flag; };
        List<Tile> tiles;
        if (component.TryGetComponent<Entity>(out var _owner))
        {
            tiles = _owner.GetAttackArea();
        }
        else
        {
            tiles = Field.GetEmptyTiles(StageManager.Instance.field.GetTiles());
        }

        
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

        tile = _tile;
        callback?.Invoke(true);
        yield break;
    }

    public override IEnumerator ExecuteSkill()
    {
        EditorLogger.Print($"Spawn {spawnData.productName}");
        var obstacle = EntityFactory.RequestEntity(spawnData, intVector2.Zero, tile);
        obstacle.team.teamNumber = PlayerID.None;
        // TODO : 팩토리를 통해 장애물을 생성하고 tile에 생성
        tile = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_SpawnWall();
        clone.spawnData = spawnData;
        return clone;
    }
}
