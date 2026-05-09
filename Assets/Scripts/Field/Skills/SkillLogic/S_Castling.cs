using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class S_Castling : BaseSkillLogic
{
    private Entity _entity;
    private Entity _target;

    public override async UniTask<bool> InputSkill(IInput input)
    {
        var all = StageManager.Instance.field.GetEntities();
        Entity entity = null;
        if(component.TryGetComponent<Entity>(out var owner))
        {
            entity = owner;
        }
        else
        {
            
            var data = await input.InputEntity(all, 1);
            if(data != null)
            {
                entity = data[0];
            }
            else
            {
                return false;
            }
        }
        var list = all.FindAll(e=>e.team.IsAlly(entity.team));
        list.Sort((a, b) => a.CurHealth.CompareTo(b.CurHealth));
        list.Remove(entity);
        
        var data2 = await input.InputEntity(list, 1);
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
        var tile1 = _entity.CurTile;
        var tile2 = _target.CurTile;
        tile1.UnsetOccupant();
        tile2.UnsetOccupant();
        yield return null;
        // TODO : 이동 코루틴으로 애니메이션 구현하기
        _target.gameObject.transform.DOMove(tile1.transform.position,0.5f);
        _entity.gameObject.transform.DOMove(tile2.transform.position,0.5f);
        yield return new WaitForSeconds(1f);
        _entity.Move(tile2);
        _target.Move(tile1);
        
        _target.Defense += 1;
        EffectFactory.Instance.Request("DefUpAura",_target.transform.position,_target.transform.lossyScale);
        
        _entity = null;
        _target = null;
        
        yield return null;
    }

    public override BaseSkillLogic Clone()
    {
        return new S_Castling();
    }
}
