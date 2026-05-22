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
        // 랜덤 기물을 선택, 해당 기물의 스킬사용이 가능한지 확인, 되면 실행, 안되면 이동가능한지 확인, 되면 실행 안되면 해당 기물 빼고 리트라이
        var list = new List<Entity>(agent.fieldEntities.FindAll(e => e.baseData.id == "Bomber"));
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list.Count <= 0) break;
            int rand = Random.Range(0, list.Count);
            var selectEntity = list[rand];
            // 선택한 기물 스킬 사용 시도6
            var result = await EnemySkillAction(selectEntity);
            // 스킬 사용 성공 시 종료
            if (result) return;
            // 기물 이동 시도
            result = await MoveForwardAction(selectEntity);
            // 이동 성공 시 종료
            if (result) return;
            // 행동할 불가 기물 제거 후 재시도
            list.RemoveAt(rand);
        }

        await base.EnemyAction();


    }

    public async UniTask<bool> MoveForwardAction(Entity entity)
    {
        if (!entity.IsControllable) return false;
        var frontList = new List<Tile>();
        foreach (var tile in entity.GetMoveArea().GetEmptyTiles())
        {
            if ((tile.fieldPos.y - entity.CurTile.fieldPos.y) * entity.direction.y >= 0)
            {
                frontList.Add(tile);
            }
        }
        agent.CreateMoveCommand(entity, frontList[Random.Range(0,frontList.Count)]);
        entity.IsControllable = false;
        return true;
    }
}
