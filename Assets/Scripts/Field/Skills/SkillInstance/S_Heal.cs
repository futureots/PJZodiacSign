using System.Reflection;
using UnityEngine;

public class S_Heal : S_BaseEntity<SD_Heal>
{
    public S_Heal(SD_Heal data) : base(data)
    {
    }

    public override void Activate()
    {
        var list = Owner.GetAttackArea();
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var other = item.occupiedObject;
            var team = Owner.GetComponent<Team>();  
            if (team.IsAlly(other.GetComponent<Team>()))
            {
                var ally = other.GetComponent<IDamageable>();
                ally.Healed(Owner.Power);
            }
        }
    }

    public override bool CanSkillInput(Field field)
    {
        return true;
    }

    public override bool IsValidInput(FieldInfo field)
    {
        return true;
    }

    public override bool SetSkillInput(Field field)
    {
        return true;
    }
}
