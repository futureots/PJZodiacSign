using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level4BossAI : EnemyAI
{
    protected override async UniTask EnemyAction()
    {
        var list = agent.actionAbleEntities;
        var bomber = list.FindAll(e => e.baseData.id == "Bomber");
        var skillBomber = bomber.Find(entity => entity.energy.IsFull());
        if (skillBomber)
        {
            // 스킬 입력
            var result = await skillBomber.skill.skillLogic.InputSkill(this);
            if (result)
            {
                // 스킬 실행
                agent.CreateSkillCommand(skillBomber.skill);
            }
        }
        // 이동 가능한 폭탄병이 있을 경우
        else if(bomber.Count>0)
        {
            var moveBomber = bomber[Random.Range(0, bomber.Count)];
            // 폭탄병은 항상 앞으로 이동하기
            foreach (var tile in moveBomber.GetMoveArea().GetEmptyTiles())
            {
                if ((tile.fieldPos.y - moveBomber.CurTile.fieldPos.y) * moveBomber.direction.y >= 0)
                {
                    agent.CreateMoveCommand(moveBomber, tile);
                    return;
                }
            }
        }
        await base.EnemyAction();


    }

    protected override IEnumerator SetRepairMode()
    {
        var list = StageManager.Instance.field.GetEntities(agent.id);
        // 남은 크레딧으로 기물 강화
        foreach (var entity in list.FindAll(e => e.baseData.id != "Bomber"))
        {
            EnhanceEntity(entity);
        }
        

        yield return null;
    }
}
