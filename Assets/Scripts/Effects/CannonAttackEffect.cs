using DG.Tweening;
using System;
using UnityEngine;

public class CannonAttackEffect : BasicAttackEffect
{

    protected override void HitAction()
    {
        onHit?.Invoke();
        EffectFactory.Instance.RequestEffect(hitEffect.name,target.transform.position,target.transform.lossyScale*3,1f);
        Destroy(gameObject);
    }
}
