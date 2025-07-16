using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{

    

    public override void SetInstantField()
    {
        base.SetInstantField();

        int cost = DataManager.Instance.playerData.stageLevel * 3;
        List<string> list = new List<string>();
        /*while (cost > 0)
        {
            // 코스트로 구매 가능한 엔티티 가져오기
            // 현재 보유 코스트에서 차감
            // 인스턴트 필드에 배치
            string name = stageData.GetRandomEntity(cost);
            list.Add(name);

            
        }*/


        // 나중에 임시데이터 말고 스테이지 별로 로직 추가하기
        foreach (var item in DataManager.Instance.playerData.handEntities)
        {
            var entity = ResourceManager.CreateEntity(item.name);
            PushEntity(entity, instantField);
        }
    }
    public override void SetMainField()
    {
        base.SetMainField();
        foreach (var tile in instantField.tiles)
        {
            if (tile.isEmpty) continue;
            Debug.Log("Occupied");
            var entity = tile.occupiedObject.GetComponent<Entity>();

            entity.MoveSequence(GameManager.Instance.field.GetTile(tile.fieldPos, isReflect), true);
        }
        
    }
}
