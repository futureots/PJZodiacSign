using DG.Tweening;
using System;
using UnityEngine;

public class CannonAttackEffect : BasicAttackEffect
{

    public override void Initialize(GameObject _target, Action action)
    {
        this.target = _target;
        onHit = action;
        Vector3 startPos = transform.position;
        Vector3 endPos = target.transform.position;

        var distance =  Vector3.Distance(startPos, endPos);
        // 중간 지점을 계산하고 높이를 더해 정점을 만듭니다.
        Vector3 midPos = (startPos + endPos) / 2f;
        midPos.y += distance / 2;
        float time = distance * speed;
        Sequence sequence = DOTween.Sequence().
            Append(transform.DOMoveX(endPos.x, time).SetEase(Ease.Linear)).
            Join(transform.DOMoveZ(endPos.z, time).SetEase(Ease.Linear)).
            Join(transform.DOMoveY(midPos.y, time/2)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo)).
            AppendCallback(HitAction);

        audioSource?.Play();
    }
    protected override void HitAction()
    {
        onHit?.Invoke();
        EffectFactory.Instance.Request(hitEffect.name,target.transform.position,target.transform.lossyScale*3,1f);
        Destroy(gameObject);
    }
}
