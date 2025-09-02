using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModeInput
{
    /// <summary>
    /// 해당 모드 세팅
    /// </summary>
    public void SetMode();

    /// <summary>
    /// 모드 세팅 제거
    /// </summary>
    public void RemoveMode();
}
