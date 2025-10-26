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
        transform.DOMove(target.transform.position+ Vector3.up*5, time).SetEase(Ease.Linear);
        // 오류로 도착 못할 경우 대비
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            other.GetComponent<IDamageable>()?.Damaged(damage);
            var hit = Instantiate(hitEffect, transform.position, Utils.QI);
            Destroy(hit, 1f);
            Destroy(gameObject);
        }
    }
}
