using UnityEngine;
using Condition;

public abstract class ConditionData : ScriptableObject, IConditionData
{
    [SerializeField]
    protected ConditionType _conditionType;
    public ConditionType conditionType { 
        get
        {
            return _conditionType;
        }
    }

    public abstract ConditionArgs QueryConditions(ConditionArgs args);
}
