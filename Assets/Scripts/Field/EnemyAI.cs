using System;
using UnityEngine;

public class EnemyAI : Agent
{

    public override void SetMode(Mode mode, Action call = null)
    {
        // AI로 계산 해서 명령 제작 후 콜백
        call?.Invoke();
    }
}
