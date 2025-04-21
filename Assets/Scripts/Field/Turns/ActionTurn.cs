using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn : ITurn
{
    public void Execute(Action onTurnEnd)
    {
        Debug.Log("행동 턴 시작");
        GameManager.Instance.turnEndButton.onClick.RemoveAllListeners();
        GameManager.Instance.turnEndButton.onClick.AddListener(() =>
        {
            onTurnEnd?.Invoke();
        });
        
    }
}
