using DG.Tweening;
using System;
using UnityEngine;

public class CannonAttackEffect : BasicAttackEffect
{

    protected override void HitAction()
    {
        _onHit?.Invoke();
        var hit = EffectFactory.Instance.RequestEffect(hitEffect.name,_target.transform.position,_target.transform.lossyScale);
        Destroy(hit, 1f);
        Destroy(gameObject);
    }
}
