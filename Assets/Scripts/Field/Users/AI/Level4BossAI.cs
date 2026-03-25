using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level4BossAI : EnemyAI
{
    protected override async UniTask EnemyAction()
    {
        var bomber = agent.actionAbleEntities.FindAll(e => e.baseData.id == "Bomber");
        List<Entity> skillBombers = bomber.FindAll(entity => entity.energy.IsFull());
        // 스킬 사용이 가능한 기물이 있는 경우
        if (skillBombers.Count>0)
        {
            var skillBomber = skillBombers[Random.Range(0,skillBombers.Count)];
            // 스킬 입력
            var result = await skillBomber.skill.skillLogic.InputSkill(this);
            if (result)
            {
                // 스킬 실행
                agent.CreateSkillCommand(skillBomber.skill);
                return;
            }
        }
        // 이동 가능한 폭탄병이 있을 경우
        if(bomber.Count>0)
        {
            var moveBomber = bomber[Random.Range(0, bomber.Count)];
            // 폭탄병은 항상 앞으로 이동하기
            var frontList = new List<Tile>();
            foreach (var tile in moveBomber.GetMoveArea().GetEmptyTiles())
            {
                if ((tile.fieldPos.y - moveBomber.CurTile.fieldPos.y) * moveBomber.direction.y >= 0)
                {
                    frontList.Add(tile);
                }
            }
            agent.CreateMoveCommand(moveBomber, frontList[Random.Range(0,frontList.Count)]);
            agent.actionAbleEntities.Remove(moveBomber);
            return;
        }
        await base.EnemyAction();


    }
}
