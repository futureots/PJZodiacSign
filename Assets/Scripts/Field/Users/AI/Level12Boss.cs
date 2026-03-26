using Cysharp.Threading.Tasks;

public class Level12Boss : EnemyAI
{
    
    protected override async UniTask EnemyAction()
    {
        var king = agent.actionAbleEntities.Find(e => e.baseData.id == "RuinedKing");
        if (king)
        {
            if (king.energy.IsFull())
            {
                var result = await king.skill.skillLogic.InputSkill(this);
                if (result)
                {
                    agent. CreateSkillCommand(king.skill);
                    return;
                }
            }
        }
        await base.EnemyAction();
    }
}
