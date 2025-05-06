using System;
using System.Collections;

using UnityEngine;

public class AttackTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        Debug.Log("공격 턴 시작");

        GameManager.Instance.RunWithCallback(AttackCoroutine(), onTurnEnd);
    }
    public IEnumerator AttackCoroutine()
    {
        InputManager.Instance.isInputStop = true;
        // 모든 기물 공격
        foreach (var attackable in Field.Instance.GetAllAttackableObject())
        {
            attackable.Attack();
        }

        // 대기시간
        yield return new WaitForSeconds(1f);

        // 모든 캐릭터 버프 업데이트
        foreach (var tile in Field.Instance.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            if (entity == null) continue;
            entity.UpdateBuff();
            entity.RemoveBuff();
        }

        Field.Instance.CleanField();
        InputManager.Instance.isInputStop = false;
        Debug.Log("CanInput");
    }
}


