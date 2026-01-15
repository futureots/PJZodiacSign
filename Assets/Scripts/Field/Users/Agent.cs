using System;
using System.Collections.Generic;
using System.Linq;
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
    

    // 행동 포인트
    public int actionCount;
    // 설정한 커맨드 리스트
    public List<Command> inputCommands;
    
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
        inputCommands = new();
    }
    public void Init(FieldController fieldController, PlayerID teamId, int credit)
    {
        this.fieldController = fieldController;
        this.id = teamId;
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

    public Command CreateMoveCommand(Entity entity, Tile tile)
    {
        var cmd = new MoveCommand(entity, tile);
        // 해당 기물의 이동명령이 있으면 제거 후 추가
        foreach (var command in inputCommands)
        {
            if(command is MoveCommand mvCmd)
            {
                if(mvCmd.Entity == entity)
                {
                    inputCommands.Remove(command);
                    command.Delete();
                    break;
                }
            }
        }
        inputCommands.Add(cmd);
        return cmd;
    }

    public Command CreateSkillCommand(SkillComponent skill)
    {
        var cmd = new SkillCommand(skill);
        // 중복 커맨드 제거 후 추가
        foreach (var command in inputCommands)
        {
            if (command is SkillCommand skCmd)
            {
                if (skCmd.Skill == skill)
                {
                    inputCommands.Remove(command);
                    command.Delete();
                    break;
                }
            }
        }
        inputCommands.Add(cmd);
        return cmd;
    }

    public Command CreateAttackCommand(Entity entity)
    {
        var cmd = new AttackCommand(entity);
        inputCommands.Add(cmd);
        return cmd;
    }

    public Command CreateEnhanceCommand(Entity baseEntity, Entity subEntity)
    {
        var cmd = new EnhanceCommand(baseEntity, subEntity);
        inputCommands.Add(cmd);
        return cmd;
    }

    public Command CreateEndCommand()
    {
        var cmd = new EndCommand(fieldController);
        inputCommands.Add(cmd);
        return cmd;
    }


    /// <summary>
    /// FieldController에 커맨드 전송
    /// </summary>
    public void SubmitCommand()
    {
        // TODO : FieldController에 ReceiveCommand로 처리
        fieldController.commandList.AddRange(inputCommands);
        fieldController.ExecutedCommands();
        inputCommands.Clear();
    }

    List<EntityLevelData> entityLevelData;
    Dictionary<intVector2,EntityLevelData> fieldEntityData;
    public AgentData getData() {
        var data = new AgentData(Credit,entityLevelData,fieldEntityData,inventory.GetInventoryData());
        return data; 
    }

}
