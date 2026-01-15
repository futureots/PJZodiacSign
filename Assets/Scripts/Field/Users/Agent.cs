using System;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    // 플레이어 번호
    public PlayerID id;
    // 팀 번호

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
    public void Init(FieldController fieldController, PlayerID teamId, int credit)
    {
        this.fieldController = fieldController;
        this.id = teamId;
        OnInitialized?.Invoke();
        Credit = credit;
        fieldController.onPhaseStarted += (phase) =>
        {
            if(phase.phaseName == PhaseType.Battle)
            {
                var _entityLevelData = new List<EntityLevelData>();
                var list = StageManager.Instance.agentField[PlayerID.P0].GetEntities();
                foreach (var entity in list)
                {
                    _entityLevelData.Add(new EntityLevelData(entity));
                }
                entityLevelData = _entityLevelData;
                var _fieldEntityData = new Dictionary<intVector2, EntityLevelData>();
                var fieldData = StageManager.Instance.field.GetEntities(id);
                foreach (var entity in fieldData)
                {
                    _fieldEntityData.Add(entity.CurTile.fieldPos, new EntityLevelData(entity));
                }
                fieldEntityData = _fieldEntityData;
            }
        };
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

    List<EntityLevelData> entityLevelData;
    Dictionary<intVector2,EntityLevelData> fieldEntityData;
    public AgentData getData() {
        var data = new AgentData(Credit,entityLevelData,fieldEntityData,inventory.GetInventoryData());
        return data; 
    }

}
