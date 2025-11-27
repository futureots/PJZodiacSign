using UnityEngine;
using Condition;

public abstract class FilterData : ScriptableObject, IFilterData
{
    [SerializeField]
    protected ConditionType _conditionType;
    public ConditionType conditionType { 
        get
        {
            return _conditionType;
        }
    }

    public abstract ConditionArgs FilterConditions(ConditionArgs args, params ConditionArgs[] prevArgs);
}
