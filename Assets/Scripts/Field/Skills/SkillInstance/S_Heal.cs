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
            var team = Owner.GetComponent<Team>();  
            if (team.isAlly(item.GetComponent<Team>()))
            {
                var ally = item.GetComponent<Entity>();
                ally.Healed(Owner.power);
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

    public override void Reinitialize()
    {
        
    }

    public override bool SetSkillInput(Field field)
    {
        return true;
    }
}
