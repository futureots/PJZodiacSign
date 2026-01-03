
public interface IDamageable
{
    /// <summary>
    /// damage만큼 피해를 입는다.
    /// </summary>
    /// <param name="damage">피해량</param>
    public void Damaged(int damage);
    /// <summary>
    /// amount만큼 회복한다.
    /// </summary>
    /// <param name="amount">회복량</param>
    public void Healed(int amount);
    /// <summary>
    /// 체력이 0인지 확인
    /// </summary>
    /// <returns>체력이 0이면 true, 아니면 false</returns>
    public bool isZero();
    /// <summary>
    /// 사망 시 호출
    /// </summary>
    public void Dead();


}
