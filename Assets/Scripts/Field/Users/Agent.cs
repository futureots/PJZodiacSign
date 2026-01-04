using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Agent : MonoBehaviour
{
    public Team team { get; protected set; }

    public FieldController fieldController { get; protected set; }

    // 행동 포인트
    public int actionCount;
    // 설정한 커맨드 리스트
    public List<Command> commands;
    // 보유중인 기물 리스트
    public List<Entity> entities;

    protected void Awake()
    {
        commands = new();
    }

    public void Init(FieldController fieldController)
    {
        this.fieldController = fieldController;
    }

    public void CreateMoveCommand(Entity entity, Tile tile)
    {
        var cmd = new MoveCommand(entity, tile);
        commands.Add(cmd);
    }

    public void CreateSkillCommand(SkillComponent skill)
    {
        var cmd = new SkillCommand(skill);
        commands.Add(cmd);
    }

    public void CreateAttackCommand(Entity entity)
    {
        var cmd = new AttackCommand(entity);
        commands.Add(cmd);
    }

    public void CreateEnhanceCommand(Entity baseEntity, Entity subEntity)
    {
        var cmd = new EnhanceCommand(baseEntity, subEntity);
        commands.Add(cmd);
    }

    public void CreateEndCommand()
    {
        var cmd = new EndCommand();
        commands.Add(cmd);
    }


    /// <summary>
    /// FieldController에 커맨드 전송
    /// </summary>
    public void SendCommand()
    {
        fieldController.commandList.AddRange(commands);
        commands.Clear();

    }

    
    

}
