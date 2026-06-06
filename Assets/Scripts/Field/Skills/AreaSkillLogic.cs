using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AreaSkillLogic : BaseSkillLogic
{
    [SerializeField] protected Area area;

    public Area Area => area;
}
