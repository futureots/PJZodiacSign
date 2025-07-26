using System;
using UnityEngine;

public class EnemyAI : Agent
{

    public override void SetMode(Mode mode, Action call = null)
    {
        // AI로 계산 해서 명령 제작 후 콜백
        call?.Invoke();
    }


    public void SetRepairMode()
    {
        // 크레딧을 사용해 기물 구매
        
        
    }
    /// <summary>
    /// 스킬 사용이 가능한 기물이 있는지 확인하는 함수
    /// </summary>
    /// <returns>스킬 사용이 가능함</returns>
    public bool CanActiveSkill()
    {
        foreach(var item in controller.entities)
        {
            if(item.curEnergy > item.skillCost)
            {
                return true;
            }
        }
        return false;
    }
}
