using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;


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
        
        // AI 용 랜덤 값
        var random = UnityEngine.Random.Range(0, tiles.Count);
        (tiles[random], tiles[0]) = (tiles[0], tiles[random]);


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

        var obstacle = EntityFactory.Instance.Request(spawnData, intVector2.Zero, _tile, _owner.team.teamNumber);
        var levelUpEffect = EffectFactory.Instance.Request("LevelUp",obstacle.transform.position, obstacle.transform.lossyScale);
        if (levelUpEffect.TryGetComponent<GlowEffect>(out var levelUp))
        {
            if (obstacle.TryGetComponent<MeshFilter>(out var mesh))
            {
                levelUp.Init(mesh.mesh);
                levelUp.Play();
            }
        }
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
