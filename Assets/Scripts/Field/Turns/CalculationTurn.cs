using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CalculationTurn : ITurn
{
    public int TeamNumber
    {
        get { return 0; }
    }

    public void StartTurn(Action onTurnEnd)
    {
        Debug.Log("공격 턴 시작");

        GameManager.Instance.RunWithCallback(AttackCoroutine(), onTurnEnd);
    }

    public IEnumerator AttackCoroutine()
    {
        // // 현재 전투 중인 필드;
        // Field curField = GameManager.Instance.field;
        //
        // // 나중에 타일 순회 하면서 타일 효과 및 중립 장애물 효과 발동 하는 방식으로 변경(효과 업데이트도 해당 턴에 실행
        // foreach (var obj in curField.GetOccupiedObjects())
        // {
        //     if (obj.GetComponent<Team>())
        //     {
        //         continue;
        //     }
        //     var attackable = obj.GetComponent<IAttackable>();
        //     if (attackable != null)
        //     {
        //         attackable.Attack();
        //     }
        // }

        // 대기시간
        yield return new WaitForSeconds(2f);
    }
}
