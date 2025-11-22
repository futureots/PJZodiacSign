using Condition;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Composite", menuName = "Scriptable Objects/Condition/Composite")]
public class CD_Composite : ConditionData
{
    [SerializeField]
    List<ConditionData> queryList;
    public override ConditionArgs QueryConditions(ConditionArgs args)
    {
        foreach (var item in queryList)
        {
            args = item.QueryConditions(args);
        }
        return args;
    }
}
