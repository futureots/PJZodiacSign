using System;
using System.Collections;
using UnityEngine;

public class Level4BossAI : EnemyAI
{
    protected override IEnumerator EnemyAction()
    {
        var list = agent.actionAbleEntities;
        var bomber = list.Find(e => e.baseData.id == "Bomber");
        if (bomber)
        {
            // 폭탄병 스킬 우선 사용
            if (bomber.energy.CurEnergy >= bomber.energy.MaxEnergy)
            {
                // 스킬 입력
                bomber.skill.skillLogic.InputSkill(this);
                // 스킬 실행
                agent.CreateSkillCommand(bomber.skill);
            }
            else
            {
                // 폭탄병은 항상 앞으로 이동하기
                foreach (var tile in bomber.GetMoveArea().GetEmptyTiles())
                {
                    if ((tile.fieldPos.y - bomber.CurTile.fieldPos.y) * bomber.direction.y >= 0)
                    {
                        agent.CreateMoveCommand(bomber, tile);
                        break;
                    }
                }
            }
        }
        else
        {
            yield return base.EnemyAction();
        }


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
