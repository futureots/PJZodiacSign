using DG.Tweening;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    GameObject target;
    int damage;
    [SerializeField]
    GameObject hitEffect;
    public float speed;
    public void Initialize(GameObject target, int damage)
    {
        this.target = target;
        this.damage = damage;
        float time = (target.transform.position - transform.position).magnitude*speed;
        Sequence sequence = DOTween.Sequence().
            Append(transform.DOMove(target.transform.position + Vector3.up * 7, time).SetEase(Ease.Linear)).
            AppendCallback(() => Destroy(gameObject));            // 도착 시 부딪히지 않아도 삭제

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            other.GetComponent<IDamageable>()?.Damaged(damage);
            var hit = Instantiate(hitEffect, transform.position, Utils.QI);
            Destroy(hit, 1f);
        }
    }
}
