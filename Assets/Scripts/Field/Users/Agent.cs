using System;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    // 플레이어 번호
    public PlayerID id;
    // 팀 번호
    public int teamNum;

    static Agent _localPlayer;
    public static Agent LocalPlayer
    {
        get
        {
            return _localPlayer;
        }
        set
        {
            _localPlayer = value;
            OnLocalPlayerChanged?.Invoke();
        }
    }
    public static Action OnLocalPlayerChanged;

    public FieldController fieldController { get; protected set; }

    public event Action OnInitialized;
    // 행동 포인트
    public int actionCount;
    // 설정한 커맨드 리스트
    public List<Command> commands;
    private int _credit;
    public int Credit
    {
        get
        {
            return _credit;
        }
        set
        {
            _credit = value;
            OnCreditChanged?.Invoke(Credit);
        }
    }
    public event Action<int> OnCreditChanged;

    public Inventory inventory;

    protected void Awake()
    {
        commands = new();
    }

    public void Init(FieldController fieldController, int teamId, int credit)
    {
        this.fieldController = fieldController;
        this.teamNum = teamId;
        OnInitialized?.Invoke();
        Credit = credit;
    }

    public void CreateMoveCommand(Entity entity, Tile tile, Action onDestroyed = null)
    {
        var cmd = new MoveCommand(entity, tile);
        cmd.onDestroyed += onDestroyed;
        // 해당 기물의 이동명령이 있으면 제거 후 추가
        foreach (var command in commands)
        {
            if(command is MoveCommand mvCmd)
            {
                if(mvCmd.entity == entity)
                {
                    commands.Remove(command);
                    command.Delete();
                    break;
                }
            }
        }
        commands.Add(cmd);
    }

    public void CreateSkillCommand(SkillComponent skill, Action onDestroyed = null)
    {
        var cmd = new SkillCommand(skill);
        cmd.onDestroyed += onDestroyed;
        // 중복 커맨드 제거 후 추가
        foreach (var command in commands)
        {
            if (command is SkillCommand skCmd)
            {
                if (skCmd._skill == skill)
                {
                    commands.Remove(command);
                    command.Delete();
                    break;
                }
            }
        }
        commands.Add(cmd);
    }

    public void CreateAttackCommand(Entity entity, Action onDestroyed = null)
    {
        var cmd = new AttackCommand(entity);
        cmd.onDestroyed += onDestroyed;
        commands.Add(cmd);
    }

    public void CreateEnhanceCommand(Entity baseEntity, Entity subEntity, Action onDestroyed = null)
    {
        var cmd = new EnhanceCommand(baseEntity, subEntity);
        cmd.onDestroyed += onDestroyed;
        commands.Add(cmd);
    }

    public void CreateEndCommand(Action onDestroyed = null)
    {
        var cmd = new EndCommand();
        cmd.onDestroyed += onDestroyed;
        commands.Add(cmd);
    }


    /// <summary>
    /// FieldController에 커맨드 전송
    /// </summary>
    public void SendCommand()
    {
        fieldController.commandList.AddRange(commands);
        commands.Clear();
        fieldController.ExecutedCommands();
    }


    public AgentData getData() { return new AgentData(); }

}
