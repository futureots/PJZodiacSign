using DG.Tweening;
using System;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    protected GameObject target;
    protected Action onHit;
    [SerializeField] protected GameObject hitEffect;
    public float speed;
    [SerializeField] protected AudioSource audioSource;

    /// <summary>
    /// target에 적중했을 때 action 실행
    /// </summary>
    /// <param name="_target">대상</param>
    /// <param name="action">적중 시 실행할 함수</param>
    public virtual void Initialize(GameObject _target, Action action)
    {
        this.target = _target;
        onHit = action;
        float time = (_target.transform.position - transform.position).magnitude * speed;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMove(_target.transform.position + Vector3.up * 7, time).SetEase(Ease.Linear))
            .AppendCallback(HitAction);
        
        audioSource?.Play();
    }

    protected virtual void HitAction()
    {
        onHit?.Invoke();
        
        Destroy(gameObject);
    }
    
}
