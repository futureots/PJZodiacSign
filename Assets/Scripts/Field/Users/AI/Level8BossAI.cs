using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Level8BossAI : EnemyAI
{
    protected override async UniTask EnemyAction()
    {
        foreach (var unicorn in agent.actionAbleEntities.Where(e=>e.baseData.id == "Unicorn"))
        {
            if (unicorn.energy.IsFull())
            {
                // 스킬 입력
                var result = await unicorn.skill.skillLogic.InputSkill(this);
                if(result)
                {
                    // 스킬 실행
                    agent.CreateSkillCommand(unicorn.skill);
                    return;
                }
            }
        }
        await base.EnemyAction();
    }
}
