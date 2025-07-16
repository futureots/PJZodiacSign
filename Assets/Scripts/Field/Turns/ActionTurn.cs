using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn : ITurn
{
    EntityController entityController;
    public ActionTurn(EntityController controller)
    {
        entityController = controller;
    }
    public void Execute(Action onTurnEnd)
    {
        Debug.Log(entityController.tag + "행동 턴 시작");
        // 현재 플레이어의 행동 턴 일 경우
        if (entityController == InputManager.Instance.controller)
        {
            InputManager.Instance.SetInputMode(InputManager.Mode.Move);
            InputManager.Instance.turnEndButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RunWithCallback(ActionCoroutine(), onTurnEnd);
                InputManager.Instance.turnEndButton.onClick.RemoveAllListeners();
            });
        }
        else
        {
            // 적 턴 행동 추가 함수 필요
            GameManager.Instance.RunWithCallback(ActionCoroutine(), onTurnEnd);
        }

    }
    public IEnumerator ActionCoroutine()
    {
        Debug.Log(entityController.tag +" 행동 턴 실행");
        InputManager.isInputStop = true;
        InputManager.Instance.SetInputMode(InputManager.Mode.None);
        

        var cmd = entityController.curCmd;
        if (cmd == null) Debug.Log("No Command");
        
        cmd?.Execute();
        entityController.curCmd = null;

        yield return new WaitForSeconds(0.1f);
        InputManager.isInputStop = false;
    }
}
