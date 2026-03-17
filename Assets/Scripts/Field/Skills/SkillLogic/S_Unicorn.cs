using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Unicorn : BaseSkillLogic
{
    private Entity _owner;
    
    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            _owner = entity;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _owner = data[0];
            return true;
        }
        
        return false;

    }

    public override IEnumerator ExecuteSkill()
    {
        var list = StageManager.Instance.field.GetEntities(_owner.team.teamNumber);
        
        // 동일 기물 즉시 공격
        var equalList = list.FindAll(e=> e.baseData.id == _owner.baseData.id);
        foreach (var e in equalList)
        {
            component.StartCoroutine(e.Attack());
        }
        
        yield return new WaitForSeconds(1.5f);
        
        
        // 진화할 기물 찾기
        var evoList = list.FindAll(e => e.baseData.id == "Knight");
        if (evoList.Count > 0)
        {
            var entity =  evoList[0];
            var dir = entity.direction;
            var tile = entity.CurTile;
            var level = entity.Level;
            var team = entity.team.teamNumber;
        
            entity.CurTile.UnsetOccupant();
            entity.Dead();
            entity = null;
        
        
            var promotion = EntityFactory.Instance.RequestEntity(_owner.baseData, dir, tile, level);
            promotion.team.teamNumber = team;
        
            // 이펙트 재생
            var levelUpEffect = EffectFactory.Instance.RequestEffect("Change",promotion.transform.position, promotion.transform.lossyScale);
            if (levelUpEffect.TryGetComponent<GlowEffect>(out var change))
            {
                if (promotion.TryGetComponent<MeshFilter>(out var mesh))
                {
                    change.Init(mesh.mesh);
                    change.Play();
                }
            }

            yield return new WaitForSeconds(1f);
        }

        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_Unicorn();
        return clone;
    }
}
