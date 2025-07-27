using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Agent
{

    public override void SetMode(Mode mode, Action call = null)
    {
        // AI로 계산 해서 명령 제작 후 콜백
        switch (mode)
        {
            case Mode.Repair:
                SetRepairMode();
                break;
            case Mode.Move:
                break;
            case Mode.Active:
                break;
            case Mode.None:
                break;
            default:
                break;
        }
        call?.Invoke();
    }


    public void SetRepairMode()
    {
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
    public void SetActionMode()
    {
        // 스킬을 사용할 수 있을 경우 스킬을 우선적으로 사용(스킬의 입력값을 넣을 수 없으면 해당 기물 빼고 재 판별
        if(CanActiveSkill(out var list))
        {
            int rand = UnityEngine.Random.Range(0, list.Count);
            controller.CreateCommand(list[rand].skillInstance);
        }
        // 스킬을 사용할 수 있는 기물이 없으면 이동한다.
        


    }
    /// <summary>
    /// 스킬 사용이 가능한 기물이 있는지 확인하는 함수
    /// </summary>
    /// <returns>스킬 사용이 가능함</returns>
    public bool CanActiveSkill(out List<Entity> Entities)
    {
        Entities = new List<Entity>();
        foreach(var item in controller.entities)
        {
            if(item.curEnergy > item.skillCost)
            {
                Entities.Add(item);
            }
        }
        if (Entities.Count > 0) return true;
        return false;
    }
}
