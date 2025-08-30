using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : Agent
{
    public ShopTable shopTable;

    public override void SetActionTurn(Action call)
    {
        this.RunWithCallback(SetActionMode(), call);
    }
    public override void SetRepairPhase(int level, Action call)
    {
        // 레벨에 맞는 데이터 가져와서 세팅하는 기능 추가 필요
        // 보유 크레딧으로 기물 랜덤 구매하기
        Debug.Log("Enemy Credit : " + Credit);
        int loopCount = 30;
        for(int i=0;i<loopCount;i++)
        {
            if (shopTable.TryGetBuyableEntity(Credit, out var entity))
            {
                //Debug.Log($"{Credit} : entity : {entity.normalPrice}");
                Credit -= entity.normalPrice;
                data.handEntities.Add(new EntityLevelData(entity));
            }
            else break;
            Credit--;
        }
        controller.SetInstantField(data.handEntities);
        controller.SetMainField(data.fieldEntities);
        this.RunWithCallback(SetRepairMode(), call);
    }
    public override void EndRepairPhase()
    {
        controller.UpdateEntities();
        base.EndRepairPhase();
    }


    public IEnumerator SetRepairMode()
    {
        yield return null;
        // 크레딧을 사용해 기물 구매 및 내 필드에 배치

        // 내 필드에 있는 기물을 메인 필드에 배치
        var list = controller.instantField.GetOccupiedObjects();
        var fieldTiles = Field.GetEmptyTile(GameManager.Instance.field.GetHalfTiles(controller.isReflect));
        foreach (var obj in list)
        {
            var entity = obj.GetComponent<Entity>();
            if(entity == null) continue;

            // 빈 타일 중 랜덤 위치 선택
            Tile tile = fieldTiles[UnityEngine.Random.Range(0, fieldTiles.Count)];
            
            // 선택한 위치에 기물 이동
            controller.PlaceOnMainField(entity,tile);
        }
        
    }

    public IEnumerator SetActionMode()
    {
        yield return new WaitForSeconds(0.5f);
        // 스킬을 사용할 수 있을 경우 스킬을 우선적으로 사용(스킬의 입력값을 넣을 수 없으면 해당 기물 빼고 재 판별)
        if(CanActiveSkill(out var list))
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            var skill = list[rand].GetSkillInstance();
            skill.SetSkillInput(GameManager.Instance.field);
            controller.CreateCommand(skill);
            yield break;
        }

        // 스킬을 사용할 수 있는 기물이 없으면 이동한다.
        
        int max = 0;
        Entity bestEntity = null;
        intVector2 bestPos = new intVector2(-1,-1);
        
        foreach (var checkEntity in controller.entities)
        {
            // 필드 값 가져오기
            int[,] field = GameManager.Instance.field.GetFieldState();

            // 현재 위치를 비우기
            var entityPos = checkEntity.curTile.fieldPos;
            field[entityPos.y, entityPos.x] = 0;
            // 적의 공격범위 가져오기 및 예상 데미지 계산
            var values = GameManager.Instance.field.CalculateEnemyThreat(field, checkEntity.team.teamNumber);

            int value;
            intVector2 pos;
            // 가장 좋은 위치의 행동 가져오기
            (value ,pos) = checkEntity.GetBestMove(field, values);
            Debug.Log($"Best Entity : {checkEntity.name} , BestPos : {pos} , Value : {value}");

            if (pos.y == -1) continue;
            // 같은 값일 경우 전의 명령만 가짐
            if(max < value || bestEntity == null)
            {
                max = value;
                bestEntity = checkEntity;
                bestPos = pos;
            }
        }
        controller.CreateCommand(bestEntity, GameManager.Instance.field.GetTile(bestPos));
    }

    /// <summary>
    /// 스킬 사용이 가능한 기물이 있는지 확인하는 함수
    /// </summary>
    /// <returns>스킬 사용이 가능함</returns>
    bool CanActiveSkill(out List<Entity> Entities)
    {
        
        Entities = new List<Entity>();
        foreach (var item in controller.entities)
        {
            if (item.skillData == null) continue;
            if (item.CurEnergy > item.SkillCost)
            {
                if (item.GetSkillInstance().CanSkillInput(GameManager.Instance.field))
                {
                    Entities.Add(item);
                }
            }
        }
        if (Entities.Count > 0) return true;
        return false;
    }


}
