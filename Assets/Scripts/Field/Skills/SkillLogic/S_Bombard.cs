using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class S_Bombard : BaseSkillLogic
{
    [SerializeField] private Area area;
    [SerializeField] private BasicAttackEffect attackEffect;
    private Entity _owner;

    public void Init(Area area, BasicAttackEffect attackEffect)
    {
        this.area = area;
        this.attackEffect = attackEffect;
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

    public override IEnumerator ExecuteSkill()
    {
        var tiles = _owner.GetAttackArea();
        var field = StageManager.Instance.field;
        int damage = _owner.Power;
        foreach (var tile in tiles)
        {
            if(tile.IsEmpty) continue;
            var target = tile.occupiedEntity;
            if (!target.team.IsAlly(_owner.team))
            {
                void Hit()
                {
                    target.Damaged(damage);
                }
                //TODO : 폭발 이펙트 재생
                var effect = Object.Instantiate(attackEffect, _owner.transform.position + Vector3.up * 7, Utils.QI);
                effect?.Initialize(target.gameObject, Hit);
                // 포격 이펙트 내부에 광역 피해를 입히는 기능 추가
                /*var splashTiles = field.GetTiles(area.GetVectors(field.GetFieldState(),tile.fieldPos,_owner.direction));
            

            
                foreach (var splashTile in splashTiles)
                {
                    if(splashTile.IsEmpty) continue;
                    if (!splashTile.occupiedEntity.team.IsAlly(_owner.team))
                    {
                        splashTile.occupiedEntity.Damaged(_owner.Power);
                    }
                }*/
            }
        }
        // TODO : 폭발 이펙트 끝날때까지 대기
        
        _owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_Bombard();
        clone.Init(area,attackEffect);
        return clone;
    }
}
