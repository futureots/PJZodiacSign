using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class S_Bombard : AreaSkillLogic
{
    private Entity _owner;

    public void Init(Area area)
    {
        this.area = area;
    }
    
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

    public override bool IsValuable()
    {
        if (component.TryGetComponent<Entity>(out var entity))
        {
            _owner = entity;
            var tiles = _owner.GetAttackArea();
            // 적이 1명이라도 있으면 사용
            return tiles.Exists(tile => !tile.IsEmpty);
        }

        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetAttackArea();
        var field = StageManager.Instance.field;
        int damage = _owner.Power;
        var direction = _owner.direction;
        var team = _owner.team;
        foreach (var tile in tiles)
        {
            if(tile.IsEmpty) continue;
            var target = tile.occupiedEntity;
            if (!target.team.IsAlly(_owner.team))
            {
                
                void Hit()
                {
                    target.Damaged(damage);
                    target.Defense -= 1;
                    var splashTiles = field.GetTiles(area.GetVectors(field.GetFieldState(),tile.fieldPos,direction));
                    foreach (var splashTile in splashTiles)
                    {
                        if(splashTile.IsEmpty) continue;
                        var splashEntity = splashTile.occupiedEntity;
                        if (!splashEntity.team.IsAlly(team))
                        {
                            splashEntity.Damaged(damage);
                            splashEntity.Defense -= 1;
                        }
                    }
                }

                var effect = EffectFactory.Instance.Request("CannonAttackEffect",_owner.transform.position + Vector3.up * 7,_owner.transform.lossyScale,5f);
                if (effect.TryGetComponent(out BasicAttackEffect atkObj))
                {
                    atkObj.Initialize(target.gameObject, Hit);
                }
                
            }
        }
        // TODO : 폭발 이펙트 끝날때까지 대기
        yield return new WaitForSeconds(2f);
        
        _owner = null;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_Bombard();
        clone.Init(area);
        return clone;
    }
}
