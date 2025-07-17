using System;
using UnityEngine;

public class EnemyAI : Agent
{

    public EntityController controller;
    public override Command GetCommand()
    {
        var cmd = controller.curCmd;
        return cmd;
    }

    public override void SetMode(Mode mode, Action call = null)
    {
        call?.Invoke();
    }
    private void Start()
    {
        team = GetComponent<Team>();
    }
}
