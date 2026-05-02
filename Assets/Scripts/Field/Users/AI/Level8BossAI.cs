using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level8BossAI : EnemyAI
{
    protected override async UniTask EnemyAction()
    {
        // 우선 유니콘 먼저 확인
        var list = new List<Entity>(agent.fieldEntities.FindAll(e => e.baseData.id == "Unicorn"));
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list.Count < 0) break;
            int rand = Random.Range(0, list.Count);
            var selectEntity = list[rand];
            // 선택한 기물 스킬 사용 시도
            var result = await EnemySkillAction(selectEntity);
            // 스킬 사용 성공 시 종료
            if (result) return;
            // 행동할 불가 기물 제거 후 재시도
            list.RemoveAt(rand);
        }

        await base.EnemyAction();
    }
}
