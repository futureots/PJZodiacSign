using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Promotion : BaseSkillLogic
{
    private Entity _entity;
    private Entity _target;

    public override async UniTask<bool> InputSkill(IInput input)
    {
        Entity entity = null;
        if(component.TryGetComponent<Entity>(out var owner))
        {
            entity = owner;
        }
        else
        {
            var list = StageManager.Instance.field.GetEntities();
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
        var teamList = StageManager.Instance.field.GetEntities(entity.team.teamNumber);
        teamList.Remove(entity);
        
        var data2 = await input.InputEntity(teamList, 1);
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
        
        
        var promotion = EntityFactory.Instance.RequestEntity(_target.baseData, dir, tile, level);
        promotion.team.teamNumber = team;
        
        // 이펙트 재생
        var levelUpEffect = EffectFactory.Instance.RequestEffect("LevelUp",promotion.transform.position, promotion.transform.lossyScale);
        if (levelUpEffect.TryGetComponent<GlowEffect>(out var levelUp))
        {
            if (promotion.TryGetComponent<MeshFilter>(out var mesh))
            {
                levelUp.Init(mesh.mesh);
                levelUp.Play();
            }
        }

        yield return new WaitForSeconds(1f);

        
        _entity = null;
        _target = null;
        
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Promotion();
    }
}
