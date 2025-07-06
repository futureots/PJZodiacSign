using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageCostData", menuName = "Scriptable Objects/StageCostData")]
public class StageCostData : ScriptableObject
{
    public StageCost GetStageCost(int level)
    {
        var cost = level * 3 + 5;
        var result = new StageCost();
        result.cost1 = (int)(cost / 2f * 0.25);
        result.cost2 = cost - result.cost1;

        return result;
        
    }
}
[System.Serializable]
public struct StageCost
{
    // 일단 폰, (룩,나,비), (킹,퀸) 에 대한 코스트 저장
    public int cost1, cost2, cost3;

    public StageCost Clone()
    {
        var clone = new StageCost();
        clone.cost1 = cost1;
        clone.cost2 = cost2;
        clone.cost3 = cost3;
        return clone;
    }
}
