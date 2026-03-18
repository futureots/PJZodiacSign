using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Level8BossAI : EnemyAI
{
    private int count = 0;
    protected override async UniTask EnemyAction()
    {
        count++;
        if (count == 3)
        {
            count = 0;
            await EnemyMoveAction();
            return;
        }
        else
        {
            var skillUnicorns = agent.actionAbleEntities.FindAll(e=>e.baseData.id == "Unicorn" && e.energy.IsFull());
            var unicorn = skillUnicorns[Random.Range(0, skillUnicorns.Count())];
            // 스킬 입력
            var result = await unicorn.skill.skillLogic.InputSkill(this);
            if(result)
            {
                // 스킬 실행
                agent.CreateSkillCommand(unicorn.skill);
                return;
            }

            await base.EnemyAction();
        }

        
        
    }
}
