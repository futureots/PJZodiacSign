using DG.Tweening;
using System;
using UnityEngine;

public class CannonAttackEffect : BasicAttackEffect
{

    protected override void HitAction()
    {
        _onHit?.Invoke();
        EffectFactory.Instance.RequestEffect(hitEffect.name,_target.transform.position,_target.transform.lossyScale,1f);
        Destroy(gameObject);
    }
}
