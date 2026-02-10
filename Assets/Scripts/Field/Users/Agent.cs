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
    
    private int _currentActionCount;

    public int CurrentActionCount
    {
        get
        {
            return  _currentActionCount;
        }
        private set
        {
            _currentActionCount = value;
            onActionCountChanged?.Invoke(_currentActionCount);
        }
    }
    public event Action<int> onActionCountChanged;

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

    
    public void Init(FieldController fieldController, PlayerID teamId, int credit)
    {
        this.fieldController = fieldController;
        this.id = teamId;
        Credit = credit;
        fieldController.onPhaseStarted += OnPhaseChange;
        fieldController.OnTurnStarted += OnTurnChange;
    }
    void OnTurnChange(Turn turn)
    {
        if(turn.agentID == id)
        {
            switch (turn.type)
            {
                case TurnType.ACTION:
                    CurrentActionCount = actionCount;
                    break;
                case TurnType.ATTACK:
                case TurnType.REPAIR:
                    CurrentActionCount = -1;
                    break;
            }
        }
    }

    void OnPhaseChange(Phase phase)
    {
        if (phase.phaseName == PhaseType.Battle)
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
    }
    
    public Command CreateMoveCommand(Entity entity, Tile tile, bool isWarp = false)
    {
        CurrentActionCount -= 1;
        var cmd = new MoveCommand(entity, tile, isWarp);
        // 해당 기물의 이동명령이 있으면 제거 후 추가
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateSkillCommand(SkillComponent skill)
    {
        CurrentActionCount -= 1;
        var cmd = new SkillCommand(skill);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateAttackCommand(Entity entity)
    {
        CurrentActionCount -= 1;
        var cmd = new AttackCommand(entity);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateEnhanceCommand(Entity target, Entity source)
    {
        CurrentActionCount -= 1;
        var cmd = new EnhanceCommand(target, source);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateEndCommand()
    {
        var cmd = new EndCommand(fieldController);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }


    List<EntityLevelData> entityLevelData;
    Dictionary<intVector2,EntityLevelData> fieldEntityData;
    public AgentData getData() {
        var data = new AgentData(Credit,entityLevelData,fieldEntityData,inventory.GetInventoryData());
        return data; 
    }

}
