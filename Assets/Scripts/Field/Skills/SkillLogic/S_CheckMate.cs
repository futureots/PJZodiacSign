using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class S_CheckMate : BaseSkillLogic
{
    [SerializeField] int damage;
    Entity owner;

    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        isContinued = false;
        if (component.TryGetComponent<Entity>(out var _owner))
        {
            owner = _owner;
            callback?.Invoke(true);
            yield break;
        }
        else
        {
            var list = StageManager.Instance.field.GetEntities();
            Entity _target = null;
            Action<Entity> action = (x) =>
            {
                _target = x;
            };
            Action<bool> conti = (flag) => { isContinued = flag; };
            yield return component.StartCoroutine(input.InputEntity(list, action, conti, 1));
            if (!isContinued)
            {
                callback?.Invoke(false);
                yield break;
            }
            owner = _target;
            callback?.Invoke(true);
        }

    }

    public override IEnumerator ExecuteSkill()
    {
        var tiles = owner.GetAttackArea();
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            if (tile.occupiedObject.TryGetComponent<Entity>(out var entity))
            {
                if (!entity.team.IsAlly(owner.team))
                {
                    entity.Damaged(damage);
                }
                
            }
        }
        owner = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        var clone = new S_CheckMate();
        clone.damage = damage;
        return clone;
    }
}
