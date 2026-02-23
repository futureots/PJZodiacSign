using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_SpawnWall : BaseSkillLogic
{
    [SerializeField] EntityData spawnData;
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
        
        var tiles = owner.GetMoveArea();
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
        EditorLogger.Print($"Spawn {spawnData.productName}");
        var obstacle = EntityFactory.RequestEntity(spawnData, intVector2.Zero, _tile,_owner.Level);
        obstacle.team.teamNumber = _owner.team.teamNumber;
        // TODO : 팩토리를 통해 장애물을 생성하고 tile에 생성
        yield return new WaitForSeconds(0.5f);
        _tile = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_SpawnWall();
        clone.spawnData = spawnData;
        return clone;
    }
}
