using System;
using System.Collections;

using UnityEngine;

public class AttackTurn : ITurn
{
    EntityController entityController;
    public AttackTurn(EntityController controller)
    {
        entityController = controller;
    }

    public void Execute(Action onTurnEnd)
    {
        Debug.Log("공격 턴 시작");

        GameManager.Instance.RunWithCallback(AttackCoroutine(), onTurnEnd);
    }
    public IEnumerator AttackCoroutine()
    {
        // 현재 전투 중인 필드;
        Field curField = GameManager.Instance.field;
        InputManager.isInputStop = true;
        // 해당 팀 기물만 공격
        foreach (var obj in curField.GetOccupiedObjects())
        {
            if(obj.tag != entityController.tag)
            {
                var attackable = obj.GetComponent<IAttackable>();
                if(attackable != null)
                {
                    attackable.Attack();
                }
            }
        }

        // 대기시간
        yield return new WaitForSeconds(1f);

        // 모든 캐릭터 버프 업데이트
        foreach (var tile in curField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            if (entity == null) continue;
            entity.UpdateBuff();
            entity.RemoveBuff();
        }

        curField.CleanField();
        InputManager.isInputStop = false;
        //Debug.Log("CanInput");
    }
}


