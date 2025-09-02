using UnityEngine;

public interface IUsable
{
    /// <summary>
    /// 데이터가 포함한 스킬 데이터의 인스턴스를 생성 후 반환
    /// </summary>
    /// <returns></returns>
    public IActive GetUseEffect();
}
