using System;
using UnityEngine;

public class EnemyAI : Agent
{

    public EntityController controller;
    public override Command GetCommand()
    {
        var cmd = controller.curCmd;
        controller.curCmd = null;
        return cmd;
    }

    public override void SetMode(Mode mode, Action call = null)
    {
        
    }
}
