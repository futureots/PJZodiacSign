using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        Debug.Log("행동 턴 시작");
        InputManager.Instance.AllocateMoveCommand();
        GameManager.Instance.turnEndButton.onClick.RemoveAllListeners();
        GameManager.Instance.turnEndButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RunWithCallback(ActionCoroutine(),onTurnEnd);
        });
        
    }
    public IEnumerator ActionCoroutine()
    {
        InputManager.Instance.isInputStop = true;
        foreach (EntityController controller in GameManager.Instance.controllers)
        {
            var cmd = controller.curCmd;
            cmd.Execute();
            controller.curCmd = null;
            yield return new WaitForSeconds(1f);
        }
        InputManager.Instance.isInputStop = false;
    }
}
