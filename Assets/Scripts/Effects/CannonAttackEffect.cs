using DG.Tweening;
using System;
using UnityEngine;

public class CannonAttackEffect : BasicAttackEffect
{

    protected override void HitAction()
    {
        _onHit?.Invoke();
        var hit = Instantiate(hitEffect, _target.transform.position, Utils.QI);
        hit.transform.localScale = Vector3.Scale(hit.transform.localScale, _target.transform.lossyScale);
        Destroy(hit, 1f);
        Destroy(gameObject);
    }
}
