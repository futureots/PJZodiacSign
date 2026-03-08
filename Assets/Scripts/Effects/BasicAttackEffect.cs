using DG.Tweening;
using System;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    protected GameObject _target;
    protected Action _onHit;
    [SerializeField] protected GameObject hitEffect;
    public float speed;

    /// <summary>
    /// target에 적중했을 때 action 실행
    /// </summary>
    /// <param name="target">대상</param>
    /// <param name="action">적중 시 실행할 함수</param>
    public void Initialize(GameObject target, Action action)
    {
        this._target = target;
        _onHit = action;
        float time = (target.transform.position - transform.position).magnitude * speed;
        Sequence sequence = DOTween.Sequence().
            Append(transform.DOMove(target.transform.position + Vector3.up * 7, time).SetEase(Ease.Linear)).
            AppendCallback(HitAction);            // 도착 시 부딪히지 않아도 삭제
    }

    protected virtual void HitAction()
    {
        _onHit?.Invoke();
        var hit = Instantiate(hitEffect, transform.position, Utils.QI);
        hit.transform.localScale = Vector3.Scale(hit.transform.localScale, _target.transform.lossyScale);
        Destroy(hit, 1f);
        Destroy(gameObject);
    }
    
}
